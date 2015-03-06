using System;
using System.Windows;
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace SimXIV
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public Thread Simulation = null;
        public static string logstring = "";
        public static double[] GCDLookup = null;
        public static Random rand = new Random();

        public MainWindow()
        {
            InitializeComponent();
        }

        public void Simulate()
        {
            logstring = "";
            bool debug = true;
            int inputIterations = 0;
            double inputFightlength = 0;
            GCDLookup = InitGCDLookup();

            Dispatcher.Invoke((Action)(() =>
            {
                simulateButton.IsEnabled = false;
                inputIterations = Convert.ToInt32(iterationsinput.Text);
                inputFightlength = Convert.ToInt32(fightLengthInput.Text);
            }));
                if (debug)
                {
                    logstring += ("Iterations:\t" + inputIterations + Environment.NewLine +
                                       "Fight Length:\t" + inputFightlength + Environment.NewLine + Environment.NewLine);
                }

            BasePlayer playerinput = GetPlayerInput(debug);

            List<double> DPSList = new List<double>();
            List<double> PPSList = new List<double>();

            for (int x = 1; x <= inputIterations; ++x) //Start Iteration
            {
                double fightLength = inputFightlength * ((double)rand.Next(-5000, 5001) / 100000 + 1);
                double currentDotTick = (double)rand.Next(0, 2999) / 1000.0;
                double nextVitalTick = (double)rand.Next(0, 2999) / 1000.0;
                double currentTime = 0.00;
                if (debug)
                {

                        logstring += ("Iteration: \t" + x + "\tFightLength: \t" + fightLength.ToString("F3") + Environment.NewLine +
                                           "Dot Tick starting at " + currentDotTick.ToString("F3") + Environment.NewLine +
                                           "Vital Tick starting at " + nextVitalTick.ToString("F3") + Environment.NewLine + Environment.NewLine);

                }

                Job Player = MakePlayer(playerinput);
                Player.debug = debug;
                Player.fightLength = fightLength;
                Player.rotation();

                while (currentTime < fightLength) //actual sim
                {
                    currentTime = nextTime(currentTime, Player.nextInstant, Player.nextGlobalCooldown, Player.nextAutoAttack, Player.finishCastAt, nextVitalTick, currentDotTick, Player.nextFallOff);
                    Player.currentTime = currentTime;
                    Player.currentDotTick = currentDotTick;
                    Player.nextVitalTick = nextVitalTick;
                    if (currentTime < fightLength)
                    {

                        if (currentTime == currentDotTick)
                        {
                            Player.tick();
                            if (Player.nextGlobalCooldown <= currentTime)
                            {
                                Player.nextGlobalCooldown += 0.0001;
                            }
                            currentDotTick += 3;
                        }
                        if (currentTime == nextVitalTick)
                        {
                            if (Player.nextGlobalCooldown <= currentTime)
                            {
                                Player.nextGlobalCooldown += 0.0001;
                            }
                            Player.lastVitalTick = currentTime;
                            Player.regen();
                            nextVitalTick += 3;
                        }
                        if (currentTime == Player.nextFallOff)
                        {
                            foreach (DoT dot in Player.activeDoTs) 
                            {
                                if (dot.fallsOffAt <= currentTime)
                                {
                                    if (debug)
                                    {
                                        log(currentTime.ToString("F3") + "\t-\t" + dot.name + " is falling off.");
                                    }
                                    Player.activeDoTs.Remove(dot);
                                    break;
                                }
                            }
                            foreach (Buff buff in Player.activeBuffs)
                            {
                                if (buff.fallsOffAt <= currentTime)
                                {
                                    if (debug)
                                    {
                                        log(currentTime.ToString("F3") + "\t-\t" + buff.name + " is falling off.");
                                    }
                                    Player.activeBuffs.Remove(buff);
                                    break;
                                }
                            }
                            if (Player.nextGlobalCooldown <= currentTime)
                            {
                                Player.nextGlobalCooldown += 0.0001;
                            }
                            Player.decrement();
                        }
                        Player.rotation();

                    }
                }
                if (debug)
                {
                    log(Environment.NewLine + Environment.NewLine +
                        "Total Damge:\t" + Player.totalDamage + "\tTotal Potency:\t" + Player.totalPotency + Environment.NewLine +
                        "DPS:\t\t" + ((double)Player.totalDamage / fightLength).ToString("F1") + "\t\tPPS:\t" + ((double)Player.totalPotency / fightLength).ToString("F2"));
                }
                Dispatcher.Invoke((Action)(() =>
                {
                    progressBar.Value = (((double)x / (double)inputIterations) * 100);
                }));
                if (debug) { debug = false; }
                DPSList.Add((double)Player.totalDamage / fightLength);
                PPSList.Add((double)Player.totalPotency / fightLength);

            }

            double DPSAvg = DPSList.Average();
            double PPSAvg = PPSList.Average();
            log(Environment.NewLine + Environment.NewLine +
                "Average DPS:\t" + DPSAvg.ToString("F2") + Environment.NewLine +
                "Average PPS:\t" + PPSAvg.ToString("F2"));
            Dispatcher.Invoke((Action)(() =>
            {
                simulateButton.IsEnabled = true;
                console.Document.Blocks.Clear();
                console.AppendText(logstring);
            }));
        }
        public static double nextTime(double currentTime, double nextinstant, double nextgcd, double nextauto, double finishcast, double nextvital, double nextdot, double nextfalloff)
        {
            List<double> valuearray = new List<double>();
            if (finishcast > currentTime)
            {
                valuearray.Add(finishcast);
            }
            else
            {
                if (nextinstant > currentTime)
                {
                    valuearray.Add(nextinstant);
                }
                if (nextgcd > currentTime)
                {
                    valuearray.Add(nextgcd);
                }
                if (nextauto > currentTime)
                {
                    valuearray.Add(nextauto);
                }
            }
            if (nextvital > currentTime)
            {
                valuearray.Add(nextvital);
            }
            if (nextdot > currentTime)
            {
                valuearray.Add(nextdot);
            }
            if (nextfalloff > currentTime)
            {
                valuearray.Add(nextfalloff);
            }

            return valuearray.Min();
        }
        public BasePlayer GetPlayerInput(bool debug)
        {
            BasePlayer p = new BasePlayer();
            Dispatcher.Invoke((Action)(() =>
            {
                p.job = StringToJob(job.Text);
                p.WDMG = Convert.ToDouble(WEP.Text);
                p.MDMG = Convert.ToDouble(MDMG.Text);
                p.AADMG = Convert.ToDouble(AADMG.Text);
                p.AADELAY = Convert.ToDouble(DELAY.Text);
                p.STR = Convert.ToDouble(STR.Text);
                p.DEX = Convert.ToDouble(DEX.Text);
                p.VIT = Convert.ToDouble(VIT.Text);
                p.MaxHP = Convert.ToInt32(MaxHP.Text);
                p.INT = Convert.ToDouble(INT.Text);
                p.MND = Convert.ToDouble(MND.Text);
                p.PIE = Convert.ToDouble(PIE.Text);
                p.MaxMP = Convert.ToInt32(MaxMP.Text);
                p.ACC = Convert.ToDouble(ACC.Text);
                p.CRIT = Convert.ToDouble(CRIT.Text);
                p.DTR = Convert.ToDouble(DTR.Text);
                p.SKS = Convert.ToDouble(SKSPD.Text);
                p.SPS = Convert.ToDouble(SPSPD.Text);
                if (debug)
                {
                    logstring += ("Job:\t" + p.job + Environment.NewLine +
                                        "WD:\t" + p.WDMG + "\tMD:\t" + p.MDMG + Environment.NewLine +
                                        "STR:\t" + p.STR + "\tDEX:\t" + p.DEX + Environment.NewLine +
                                        "INT:\t" + p.INT + "\tMND:\t" + p.MND + Environment.NewLine +
                                        "VIT:\t" + p.VIT + "\tPIE:\t" + p.PIE + Environment.NewLine +
                                        "CRIT:\t" + p.CRIT + "\tDTR:\t" + p.DTR + Environment.NewLine +
                                        "SKS:\t" + p.SKS + "\tSPS:\t" + p.SPS + Environment.NewLine +
                                        "ACC:\t" + p.ACC + Environment.NewLine + Environment.NewLine
                                        );
                }
            }));
            return p;
        }
        public JobEnum StringToJob(string jstring)
        {
            switch (jstring)
            {
                case "Black Mage": return JobEnum.BlackMage;
                default: return JobEnum.None;
            }
        }
        public Job MakePlayer(BasePlayer p)
        {
            Job j = new Job()
            {
                WDMG = p.WDMG,
                MDMG = p.MDMG,
                AADMG = p.AADMG,
                AADELAY = p.AADELAY,
                STR = p.STR,
                DEX = p.DEX,
                VIT = p.VIT,
                MaxHP = p.MaxHP,
                CurrentHP = p.MaxHP,
                INT = p.INT,
                MND = p.MND,
                PIE = p.PIE,
                MaxMP = p.MaxMP,
                CurrentMP = p.MaxMP,
                MaxTP = 1000,
                CurrentTP = 1000,
                ACC = p.ACC,
                CRIT = p.CRIT,
                DTR = p.DTR,
                SKS = p.SKS,
                SPS = p.SPS,
                nextAutoAttack = 0.0,
                nextGlobalCooldown = 0.0,
                nextInstant = 0.0,
                totalDamage = 0,
                totalCrits = 0,
                totalNonCrits = 0,
                totalSwings = 0,
                totalHits = 0,
                totalMisses = 0,
                MPGained = 0,
                MPUsed = 0,
                TPGained = 0,
                TPUsed = 0,
            finishCastAt = 0.0,
            isCasting = false,
            totalPotency = 0,
            lastVitalTick = 0.0,
            activeDoTs = new List<DoT>(),
            activeBuffs = new List<Buff>(),
            fightLength = 0, //
            currentTime =0, //
            currentDotTick=0, //
            nextVitalTick=0, //
            nextFallOff=0,
            };

            switch (j.JobName)
            {
                case JobEnum.BlackMage: return new BlackMage(j);
                default: return j;
            }
        }
        public static double CalcGCD(double sps, double[] gcdarr, double castTime)
        {
            int index = (int)sps - 341;
            double gcdreduction = gcdarr[index];
            return castTime - ((castTime / 2.5) * gcdreduction);
        }

        #region log
        public static void log(String s, bool newline = true)
        {
            logstring += s;
            if (newline) { logstring += "\n"; }
        }
        public static void writeLog()
        {
            StreamWriter sw = File.AppendText("output.txt");
            sw.WriteLine(logstring);
            sw.Close();
        }
        public static void clearLog()
        {
            StreamWriter sw = new StreamWriter("output.txt");
            sw.Write("");
            sw.Close();
        }
        public void readLog()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                StreamReader sr = new StreamReader("output.txt"); //TODO allow user to rename this.
                var readContents = sr.ReadToEnd();
                console.AppendText(readContents);
                sr.Close();
            }));
        }
        #endregion

        #region GUI
        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void applicationExit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            this.Dispatcher.Invoke((Action)(() =>
            {
                console.Document.Blocks.Clear();              // Init Console
                console.AppendText("" + Environment.NewLine); // 
            }));

            Simulation = new Thread(new ThreadStart(Simulate));
            Simulation.Name = "Simulation";
            Simulation.Start();

        }
        #endregion

        public double[] InitGCDLookup()
        {
            double[] arr = new double[] {
                0.000000,
0.001952,
0.003904,
0.005856,
0.006808,
0.007760,
0.008712,
0.009664,
0.010616,
0.011568,
0.012520,
0.013472,
0.014424,
0.015376,
0.016328,
0.017280,
0.018232,
0.019184,
0.020136,
0.021088,
0.022040,
0.022992,
0.023944,
0.024896,
0.025848,
0.026800,
0.027752,
0.028704,
0.029656,
0.030608,
0.031560,
0.032512,
0.033464,
0.034416,
0.035368,
0.036320,
0.037272,
0.038224,
0.039176,
0.040128,
0.041080,
0.042032,
0.042984,
0.043936,
0.044888,
0.045840,
0.046792,
0.047744,
0.048696,
0.049648,
0.050600,
0.051552,
0.052504,
0.053456,
0.054408,
0.055360,
0.056312,
0.057264,
0.058216,
0.059168,
0.060120,
0.061072,
0.062024,
0.062976,
0.063928,
0.064880,
0.065832,
0.066784,
0.067736,
0.068688,
0.069640,
0.070592,
0.071544,
0.072496,
0.073448,
0.074400,
0.075352,
0.076304,
0.077256,
0.078208,
0.079160,
0.080112,
0.081064,
0.082016,
0.082968,
0.083920,
0.084872,
0.085824,
0.086776,
0.087728,
0.088680,
0.089632,
0.090584,
0.091536,
0.092488,
0.093440,
0.094392,
0.095344,
0.096296,
0.097248,
0.098200,
0.099152,
0.100104,
0.101056,
0.102008,
0.102960,
0.103912,
0.104864,
0.105816,
0.106768,
0.107720,
0.108672,
0.109624,
0.110576,
0.111528,
0.112480,
0.113432,
0.114384,
0.115336,
0.116288,
0.117240,
0.118192,
0.119144,
0.120096,
0.121048,
0.122000,
0.122952,
0.123904,
0.124856,
0.125808,
0.126760,
0.127712,
0.128664,
0.129616,
0.130568,
0.131520,
0.132472,
0.133424,
0.134376,
0.135328,
0.136280,
0.137232,
0.138184,
0.139136,
0.140088,
0.141040,
0.141992,
0.142944,
0.143896,
0.144848,
0.145800,
0.146752,
0.147704,
0.148656,
0.149608,
0.150560,
0.151512,
0.152464,
0.153416,
0.154368,
0.155320,
0.156272,
0.157224,
0.158176,
0.159128,
0.160080,
0.161032,
0.161984,
0.162936,
0.163888,
0.164840,
0.165792,
0.166744,
0.167696,
0.168648,
0.169600,
0.170552,
0.171504,
0.172456,
0.173408,
0.174360,
0.175312,
0.176264,
0.177216,
0.178168,
0.179120,
0.180072,
0.181024,
0.181976,
0.182928,
0.183880,
0.184832,
0.185784,
0.186736,
0.187688,
0.188640,
0.189592,
0.190544,
0.191496,
0.192448,
0.193400,
0.194352,
0.195304,
0.196256,
0.197208,
0.198160,
0.199112,
0.200064,
0.201016,
0.201968,
0.202920,
0.203872,
0.204824,
0.205776,
0.206728,
0.207680,
0.208632,
0.209584,
0.210536,
0.211488,
0.212440,
0.213392,
0.214344,
0.215296,
0.216248,
0.217200,
0.218152,
0.219104,
0.220056,
0.221008,
0.221960,
0.222912,
0.223864,
0.224816,
0.225768,
0.226720,
0.227672,
0.228624,
0.229576,
0.230528,
0.231480,
0.232432,
0.233384,
0.234336,
0.235288,
0.236240,
0.237192,
0.238144,
0.239096,
0.240048,
0.241000,
0.241952,
0.242904,
0.243856,
0.244808,
0.245760,
0.246712,
0.247664,
0.248616,
0.249568,
0.250520,
0.251472,
0.252424,
0.253376,
0.254328,
0.255280,
0.256232,
0.257184,
0.258136,
0.259088,
0.260040,
0.260992,
0.261944,
0.262896,
0.263848,
0.264800,
0.265752,
0.266704,
0.267656,
0.268608,
0.269560,
0.270512,
0.271464,
0.272416,
0.273368,
0.274320,
0.275272,
0.276224,
0.277176,
0.278128,
0.279080,
0.280032,
0.280984,
0.281936,
0.282888,
0.283840,
0.284792,
0.285744,
0.286696,
0.287648,
0.288600,
0.289552,
0.290504,
0.291456,
0.292408,
0.293360,
0.294312,
0.295264,
0.296216,
0.297168,
0.298120,
0.299072,
0.300024,
0.300976,
0.301928,
0.302880,
0.303832,
0.304784,
0.305736,
0.306688,
0.307640,
0.308592,
0.309544,
0.310496,
0.311448,
0.312400,
0.313352,
0.314304,
0.315256,
0.316208,
0.317160,
0.318112,
0.319064,
0.320016,
0.320968,
0.321920,
0.322872,
0.323824,
0.324776,
0.325728,
0.326680,
0.327632,
0.328584,
0.329536,
0.330488,
0.331440,
0.332392,
0.333344,
0.334296,
0.335248,
0.336200,
0.337152,
0.338104,
0.339056,
0.340008,
0.340960,
0.341912,
0.342864,
0.343816,
0.344768,
0.345720,
0.346672,
0.347624,
0.348576,
0.349528,
0.350480,
0.351432,
0.352384,
0.353336,
0.354288,
0.355240,
0.356192,
0.357144,
0.358096,
0.359048,
0.360000,
0.360952,
0.361904,
0.362856,
0.363808,
0.364760,
0.365712,
0.366664,
0.367616,
0.368568,
0.369520,
0.370472,
0.371424,
0.372376,
0.373328,
0.374280,
0.375232,
0.376184,
0.377136,
0.378088,
0.379040,
0.379992,
0.380944,
0.381896,
0.382848,
0.383800,
0.384752,
0.385704,
0.386656,
0.387608,
0.388560,
0.389512,
0.390464,
0.391416,
0.392368,
0.393320,
0.394272,
0.395224,
0.396176,
0.397128,
0.398080,
0.399032,
0.399984,
0.400936,
0.401888,
0.402840,
0.403792,
0.404744,
0.405696,
0.406648,
0.407600,
0.408552,
0.409504,
0.410456,
0.411408,
0.412360,
0.413312,
0.414264,
0.415216,
0.416168,
0.417120,
0.418072,
0.419024,
0.419976,
0.420928,
0.421880,
0.422832,
0.423784,
0.424736,
0.425688,
0.426640,
0.427592,
0.428544,
0.429496,
0.430448,
0.431400,
0.432352,
0.433304,
0.434256,
0.435208,
0.436160,
0.437112,
0.438064,
0.439016,
0.439968,
0.440920,
0.441872,
0.442824,
0.443776,
0.444728,
0.445680,
0.446632,
0.447584,
0.448536,
0.449488,
0.450440,
0.451392,
0.452344,
0.453296,
0.454248,
0.455200,
0.456152,
0.457104,
0.458056,
0.459008,
0.459960,
0.460912,
0.461864,
0.462816,
0.463768,
0.464720,
0.465672,
0.466624,
0.467576,
0.468528,
0.469480,
0.470432,
0.471384,
0.472336,
0.473288,
0.474240,
0.475192,
0.476144,
0.477096,
0.478048,
0.479000,
0.479952,
0.480904,
0.481856,
0.482808,
0.483760,
0.484712,
0.485664,
0.486616,
0.487568,
0.488520,
0.489472,
0.490424,
0.491376,
0.492328,
0.493280,
0.494232,
0.495184,
0.496136,
0.497088,
0.498040,
0.498992,
0.499944,
0.500896,
0.501848,
0.502800,
0.503752,
0.504704,
0.505656,
0.506608,
0.507560,
0.508512,
0.509464,
0.510416,
0.511368,
0.512320,
0.513272,
0.514224,
0.515176,
0.516128,
0.517080,
0.518032,
0.518984,
0.519936,
0.520888,
0.521840,
0.522792,
0.523744,
0.524696,
0.525648,
0.526600,
0.527552,
0.528504,
0.529456,
0.530408,
0.531360,
0.532312,
0.533264,
0.534216,
0.535168,
0.536120,
0.537072,
0.538024,
0.538976,
0.539928,
0.540880,
0.541832,
0.542784,
0.543736,
0.544688,
0.545640,
0.546592,
0.547544,
0.548496,
0.549448,
0.550400,
0.551352,
0.552304,
0.553256,
0.554208,
0.555160,
0.556112,
0.557064,
0.558016,
0.558968,
0.559920,
0.560872,
0.561824,
0.562776,
0.563728,
0.564680,
0.565632,
0.566584,
0.567536,
0.568488,
0.569440,
0.570392,
0.571344,
0.572296,
0.573248,
0.574200,
0.575152,
0.576104,
0.577056,
0.578008,
0.578960,
0.579912,
0.580864,
0.581816,
0.582768,
0.583720,
0.584672,
0.585624,
0.586576,
0.587528,
0.588480,
0.589432,
0.590384,
0.591336,
0.592288,
0.593240,
0.594192,
0.595144,
0.596096,
0.597048,
0.598000,
0.598952,
0.599904,
0.600856,
0.601808,
0.602760,
0.603712,
0.604664,
0.605616,
0.606568,
0.607520,
0.608472,
0.609424,
0.610376,
0.611328,
0.612280,
0.613232,
0.614184,
0.615136,
0.616088,
0.617040,
0.617992,
0.618944,
0.619896,
0.620848,
0.621800,
0.622752,
0.623704,
0.624656,
0.625608,
0.626560,
0.627512,
0.628464,
0.629416,
0.630368,
0.631320,
0.632272,
0.633224,
0.634176,
0.635128,
0.636080,
0.637032,
0.637984,
0.638936,
0.639888,
0.640840,
0.641792,
0.642744,
0.643696,
0.644648,
0.645600,
0.646552,
0.647504,
0.648456,
0.649408,
0.650360,
0.651312,
0.652264,
0.653216,
0.654168,
0.655120,
0.656072,
0.657024,
0.657976,
0.658928,
0.659880,
0.660832,
0.661784,
0.662736,
0.663688,
0.664640,
0.665592,
0.666544,
0.667496,
0.668448,
0.669400,
0.670352,
0.671304,
0.672256,
0.673208,
0.674160,
0.675112,
0.676064,
0.677016,
0.677968,
0.678920,
0.679872,
0.680824,
0.681776,
0.682728,
0.683680,
0.684632,
0.685584,
0.686536,
0.687488,
0.688440,
0.689392,
0.690344,
0.691296,
0.692248,
0.693200,
0.694152,
0.695104,
0.696056,
0.697008,
0.697960,
0.698912,
0.699864,
0.700816,
0.701768,
0.702720,
0.703672,
0.704624,
0.705576,
0.706528,
0.707480,
0.708432,
0.709384,
0.710336,
0.711288,
0.712240,
0.713192,
0.714144,
0.715096,
0.716048,
0.717000,
0.717952,
0.718904,
0.719856,
0.720808,
0.721760,
0.722712,
0.723664,
0.724616,
0.725568,
0.726520,
0.727472,
0.728424,
0.729376,
0.730328,
0.731280,
0.732232,
0.733184,
0.734136,
0.735088,
0.736040,
0.736992,
0.737944,
0.738896,
0.739848,
0.740800,
0.741752,
0.742704,
0.743656,
0.744608,
0.745560,
0.746512,
0.747464,
0.748416,
0.749368,
0.750320,
0.751272,
0.752224,
0.753176,
0.754128,
0.755080,
0.756032,
0.756984,
0.757936,
0.758888,
0.759840,
0.760792,
0.761744,
0.762696,
0.763648,
0.764600,
0.765552,
0.766504,
0.767456,
0.768408,
0.769360,
0.770312,
0.771264,
0.772216,
0.773168,
0.774120,
0.775072,
0.776024,
0.776976,
0.777928,
0.778880,
0.779832,
0.780784,
0.781736,
0.782688,
0.783640,
0.784592,
0.785544,
0.786496,
0.787448,
0.788400,
0.789352,
0.790304,
0.791256,
0.792208,
0.793160,
0.794112,
0.795064,
0.796016,
0.796968,
0.797920,
0.798872,
0.799824,
0.800776,
0.801728,
0.802680,
0.803632,
0.804584,
0.805536,
0.806488,
0.807440,
0.808392,
0.809344,
0.810296,
0.811248,
0.812200,
0.813152,
0.814104,
0.815056,
0.816008,
0.816960,
0.817912,
0.818864,
0.819816,
0.820768,
0.821720,
0.822672,
0.823624,
0.824576,
0.825528,
0.826480,
0.827432,
0.828384,
0.829336,
0.830288,
0.831240,
0.832192,
0.833144,
0.834096,
0.835048,
0.836000,
0.836952,
0.837904,
0.838856,
0.839808,
0.840760,
0.841712,
0.842664,
0.843616,
0.844568,
0.845520,
0.846472,
0.847424,
0.848376,
0.849328,
0.850280,
0.851232,
0.852184,
0.853136,
0.854088,
0.855040,
0.855992,
0.856944,
0.857896,
0.858848,
0.859800,
0.860752,
0.861704,
0.862656,
0.863608,
0.864560,
0.865512,
0.866464,
0.867416,
0.868368,
0.869320,
0.870272,
0.871224,
0.872176,
0.873128,
0.874080,
0.875032,
0.875984,
0.876936,
0.877888,
0.878840,
0.879792,
0.880744,
0.881696,
0.882648,
0.883600,
0.884552,
0.885504,
0.886456,
0.887408,
0.888360,
0.889312,
0.890264,
0.891216,
0.892168,
0.893120,
0.894072,
0.895024,
0.895976,
0.896928,
0.897880,
0.898832,
0.899784,
0.900736,
0.901688,
0.902640,
0.903592,
0.904544,
0.905496,
0.906448,
0.907400,
0.908352,
0.909304,
0.910256,
0.911208,
0.912160,
0.913112,
0.914064,
0.915016,
0.915968,
0.916920,
0.917872,
0.918824,
0.919776,
0.920728,
0.921680,
0.922632,
0.923584,
0.924536,
0.925488,
0.926440,
0.927392,
0.928344,
0.929296,
0.930248,
0.931200,
0.932152,
0.933104,
0.934056,
0.935008,
0.935960,
0.936912,
0.937864,
0.938816,
0.939768,
0.940720,
0.941672,
0.942624,
0.943576,
0.944528,
0.945480,
0.946432,
0.947384,
0.948336,
0.949288,
0.950240,
0.951192,
0.952144,
0.953096,
0.954048,
0.955000,
0.955952,
0.956904,
0.957856,
0.958808,
0.959760,
0.960712,
0.961664,
0.962616,
0.963568,
0.964520,
0.965472,
0.966424,
0.967376,
0.968328,
0.969280,
0.970232,
0.971184,
0.972136,
0.973088,
0.974040,
0.974992,
0.975944,
0.976896,
0.977848,
0.978800,
0.979752,
0.980704,
0.981656,
0.982608,
0.983560,
0.984512,
0.985464,
0.986416,
0.987368,
0.988320,
0.989272,
0.990224,
0.991176,
0.992128,
0.993080,
0.994032,
0.994984,
0.995936,
0.996888,
0.997840,
0.998792,
0.999744,
1.000696,
1.001648,
1.002600,
1.003552,
1.004504,
1.005456,
1.006408,
1.007360,
1.008312,
1.009264,
1.010216,
1.011168,
1.012120,
1.013072,
1.014024,
1.014976,
1.015928,
1.016880,
1.017832,
1.018784,
1.019736,
1.020688,
1.021640,
1.022592,
1.023544,
1.024496,
1.025448,
1.026400,
1.027352,
1.028304,
1.029256,
1.030208,
1.031160,
1.032112,
1.033064,
1.034016,
1.034968,
1.035920,
1.036872,
1.037824,
1.038776,
1.039728,
1.040680,
1.041632,
1.042584,
1.043536,
1.044488,
1.045440,
1.046392,
1.047344,
1.048296,
1.049248,
1.050200,
1.051152,
1.052104,
1.053056,
1.054008,
1.054960,
1.055912,
1.056864,
1.057816,
1.058768,
1.059720,
1.060672,
1.061624,
1.062576,
1.063528,
1.064480,
1.065432
            };
            return arr;
        }
    }

}
