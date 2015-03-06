using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;


namespace SimXIV
{

    public class BlackMage : Job
    {
        public enum Stance { None, AF1, AF2, AF3, UI1, UI2, UI3 }
        public Stance stance = Stance.None;
        public CastingSpell currentCast;
        public bool firestarter = false;
        public double firestarter_avail = 0.0;
        public bool thundercloud = false;
        public CastingSpell lastSpell;
        public double timeToFullMana()
        {
            double MissingMana = MaxMP - CurrentMP;
            double ManaPerUI3Tick = MaxMP * 0.60;
            double ManaTicksNeeded = Math.Ceiling(MissingMana / ManaPerUI3Tick);
            return lastVitalTick + (ManaTicksNeeded * 3) - currentTime;
        }
        public BlackMage()
        {
            name = "Black Mage";
            classname = "Thaumaturge";
        }
        public BlackMage(Job j)
        {
            name = "Black Mage";
            classname = "Thaumaturge";
            WDMG = j.WDMG;
            MDMG = j.MDMG;
            AADMG = j.AADMG;
            AADELAY = j.AADELAY;
            STR = j.STR;
            DEX = j.DEX;
            VIT = j.VIT;
            MaxHP = j.MaxHP;
            CurrentHP = j.MaxHP;
            INT = j.INT;
            MND = j.MND;
            PIE = j.PIE;
            MaxMP = j.MaxMP;
            CurrentMP = j.MaxMP;
            MaxTP = 1000;
            CurrentTP = 1000;
            ACC = j.ACC;
            CRIT = j.CRIT;
            DTR = j.DTR;
            SKS = j.SKS;
            SPS = j.SPS;
            nextAutoAttack = 0.0;
            nextGlobalCooldown = 0.0;
            nextInstant = 0.0;
            finishCastAt = 0.0;
            isCasting = false;
            totalDamage = 0;
            totalPotency = 0;
            totalCrits = 0;
            totalNonCrits = 0;
            totalSwings = 0;
            totalHits = 0;
            totalMisses = 0;
            MPGained = 0;
            MPUsed = 0;
            TPGained = 0;
            TPUsed = 0;
            stance = Stance.None;
            currentCast = CastingSpell.None;
            firestarter = false;
            firestarter_avail = 0.0;
            thundercloud = false;
            lastSpell = CastingSpell.None;
            lastVitalTick = 0.0;
            activeDoTs.Clear();
            activeBuffs.Clear();
            fightLength = 0; //
            currentTime = 0; //
            currentDotTick = 0; //
            nextVitalTick = 0; //
            nextGlobalCooldown = 0;
            nextInstant = 0;
            nextAutoAttack = 0;
            finishCastAt = 0;
            nextFallOff = 0;
            isCasting = false;

        }
        public override void rotation()
        {
            if (isCasting)
            {
                if (currentCast == CastingSpell.FireIII && currentTime == finishCastAt)
                {
                    impact(ref fireiii);
                }
                if (currentCast == CastingSpell.Fire && currentTime == finishCastAt)
                {
                    impact(ref fire);
                }
                if (currentCast == CastingSpell.BlizzardIII && currentTime == finishCastAt)
                {
                    impact(ref blizzardiii);
                }
                if (currentCast == CastingSpell.Blizzard && currentTime == finishCastAt)
                {
                    impact(ref blizzard);
                }
                if (currentCast == CastingSpell.Thunder && currentTime == finishCastAt)
                {
                    impact(ref thunder);
                }
            }
            else
            {

                if (firestarter && stance == Stance.UI3 && MaxMP == CurrentMP && transpose.nextCast <= currentTime)
                {
                    execute(ref transpose);
                }
                if (thundercloud && (firestarter_avail > currentTime || !firestarter) && lastSpell == CastingSpell.Fire)
                {
                    execute(ref thunderiiitcp);
                }
                if ((firestarter && firestarter_avail <= currentTime && stance == Stance.AF3) || stance == Stance.AF1)
                {
                    execute(ref fireiiifsp);
                }
                if ((stance == Stance.None) || (stance == Stance.UI3 && CurrentMP == MaxMP) || (stance == Stance.UI3 && timeToFullMana() < MainWindow.CalcGCD(SPS, MainWindow.GCDLookup, 1.75) && (double)CurrentMP > (double)MaxMP * 0.4))
                {
                    if (!firestarter)
                    {
                        execute(ref fireiii);
                    }
                }
                else if (stance == Stance.UI3)
                {
                    bool thunderexists = false;
                    double thundertimeleft = 0.0;
                    foreach (DoT dot in activeDoTs)
                    {
                        if (CastingSpell.Thunder == dot.spell)
                        {
                            thunderexists = true;
                            thundertimeleft = dot.timeLeft;
                        }
                    }
                    if (!thunderexists)
                    {
                        execute(ref thunder);
                    }
                    else if (thundertimeleft < 3)
                    {
                        execute(ref thunder);
                    }
                    else
                        if (stance == Stance.UI3)
                        {
                            execute(ref blizzard);
                        }
                }
                if ((stance == Stance.AF3 && CurrentMP >= 929))
                {
                    execute(ref fire);
                }
                if ((stance == Stance.AF3 && CurrentMP < 929))
                {
                    execute(ref blizzardiii);
                }
                if (stance == Stance.UI3)
                {
                    bool thunderexists = false;
                    double thundertimeleft = 0.0;
                    foreach (DoT dot in activeDoTs)
                    {
                        if (CastingSpell.Thunder == dot.spell)
                        {
                            thunderexists = true;
                            thundertimeleft = dot.timeLeft;
                        }
                    }
                    if (!thunderexists)
                    {
                        execute(ref thunder);
                    }
                    else if (thundertimeleft < 3)
                    {
                        execute(ref thunder);
                    }
                }
            }
        }
        public override void execute(ref Ability ability)
        {
            if (!isCasting && currentTime >= nextInstant)
            {
                if (ability.aType == abilityType.Instant)
                {
                    nextGlobalCooldown = currentTime + 0.650;
                    nextInstant = currentTime + 0.650;
                    if (debug)
                    {
                        MainWindow.log(currentTime.ToString("F3") + "\t-\t" + "Executing " + ability.name + ".");
                    }
                    if (ability.thisSpell == CastingSpell.Transpose)
                    {
                        switch (stance)
                        {
                            case Stance.UI3: stance = Stance.AF1; break;
                            case Stance.UI2: stance = Stance.AF1; break;
                            case Stance.UI1: stance = Stance.AF1; break;
                            case Stance.AF3: stance = Stance.UI1; break;
                            case Stance.AF2: stance = Stance.UI1; break;
                            case Stance.AF1: stance = Stance.UI1; break;

                        }
                    }
                    ability.nextCast = ability.recastTime + currentTime;
                }
            }
            if (isCasting == false && (currentTime >= nextGlobalCooldown))
                if (ability.aType == abilityType.Spell)
                {
                    double manaCost = (double)ability.MPcost;
                    switch (stance)
                    {
                        case Stance.AF3: if (ability.aspect == Aspect.Ice) { manaCost /= 4; } else if (ability.aspect == Aspect.Fire) { manaCost *= 2; } break;
                        case Stance.AF2: if (ability.aspect == Aspect.Ice) { manaCost /= 4; } else if (ability.aspect == Aspect.Fire) { manaCost *= 2; } break;
                        case Stance.AF1: if (ability.aspect == Aspect.Ice) { manaCost /= 2; } else if (ability.aspect == Aspect.Fire) { manaCost *= 2; } break;
                        case Stance.UI3: if (ability.aspect == Aspect.Fire) { manaCost /= 4; } break;
                        case Stance.UI2: if (ability.aspect == Aspect.Fire) { manaCost /= 4; } break;
                        case Stance.UI1: if (ability.aspect == Aspect.Fire) { manaCost /= 2; } break;
                        default: break;
                    }
                    manaCost = Math.Floor(manaCost);
                    if (CurrentMP >= manaCost)
                    {

                        double castTime = MainWindow.CalcGCD(SPS, MainWindow.GCDLookup, ability.castTime);
                        double baseGCD = MainWindow.CalcGCD(SPS, MainWindow.GCDLookup, 2.5);
                        string castText = "";
                        switch (stance)
                        {
                            case Stance.AF3: if (ability.aspect == Aspect.Ice) { castTime /= 2; } break;
                            case Stance.UI3: if (ability.aspect == Aspect.Fire) { castTime /= 2; } break;
                            default: break;
                        }
                        double tempNextGCD = 0.0;
                        double tempNextInstant = 0.0;
                        if (castTime > 0.0)
                        {
                            isCasting = true;
                            finishCastAt = currentTime + castTime;
                            castText = "Begin casting";
                        }
                        else
                        {
                            castText = "Executing";
                            if (debug)
                            {
                                MainWindow.log(currentTime.ToString("F3") + "\t-\t" + castText + " " + ability.name + ".");
                            }
                            if (ability.thisSpell == CastingSpell.FSP)
                            {
                                firestarter = false;
                            }
                            else if (ability.name == "Thunder III (TCP)")
                            {
                                thundercloud = false;
                            }
                            impact(ref ability);
                        }
                        if (castTime < baseGCD && castTime > 0) { tempNextGCD = baseGCD; tempNextInstant = castTime + 0.100; } else if (castTime != 0) { tempNextGCD = castTime; tempNextInstant = castTime + 0.100; } else { tempNextGCD = baseGCD; tempNextInstant = 0.100; };
                        nextGlobalCooldown = tempNextGCD + currentTime + 0.0001;
                        nextInstant = tempNextInstant + currentTime;
                        currentCast = ability.thisSpell;
                        if (debug && castTime > 0.0)
                        {
                            MainWindow.log(currentTime.ToString("F3") + "\t-\t" + castText + " " + ability.name + ".\t End cast at " + finishCastAt.ToString("F3") + ".");
                        }
                    }
                }
        }
        public override void impact(ref Ability ability)
        {
            if (ability.aType == abilityType.Spell)
            {
                ability.hits += 1;
                totalHits += 1;
                totalSwings += 1;
                int mpbefore = CurrentMP;
                double manaCost = (double)ability.MPcost;
                double potency = (double)ability.potency;
                switch (stance)
                {
                    case Stance.AF3: if (ability.aspect == Aspect.Ice) { manaCost /= 4; potency *= 0.7; } else if (ability.aspect == Aspect.Fire) { manaCost *= 2; potency *= 1.8; } break;
                    case Stance.AF2: if (ability.aspect == Aspect.Ice) { manaCost /= 4; potency *= 0.8; } else if (ability.aspect == Aspect.Fire) { manaCost *= 2; potency *= 1.6; } break;
                    case Stance.AF1: if (ability.aspect == Aspect.Ice) { manaCost /= 2; potency *= 0.9; } else if (ability.aspect == Aspect.Fire) { manaCost *= 2; potency *= 1.4; } break;
                    case Stance.UI3: if (ability.aspect == Aspect.Fire) { manaCost /= 4; potency *= 0.7; } break;
                    case Stance.UI2: if (ability.aspect == Aspect.Fire) { manaCost /= 4; potency *= 0.8; } break;
                    case Stance.UI1: if (ability.aspect == Aspect.Fire) { manaCost /= 2; potency *= 0.9; } break;
                    default: break;
                }
                manaCost = Math.Floor(manaCost);
                double thisdamage = damage(potency);
                double critRoll = ((double)MainWindow.rand.Next(0, 1000000) / 10000.0);
                double critChance = 5 + ((CRIT - 341) / 13.84598115);
                string critText = "";
                double critMult = 1.0;
                if (critRoll < critChance)
                {
                    critText = " (Critical!)";
                    totalCrits += 1;
                    critMult = 1.5;
                }
                thisdamage *= critMult;
                thisdamage = Math.Round(thisdamage);
                MPUsed += (int)manaCost;
                CurrentMP -= (int)manaCost;
                totalDamage += (int)Math.Round(thisdamage);
                totalPotency += (int)Math.Floor(potency);

                if (isCasting)
                {
                    isCasting = false;
                    nextInstant = currentTime + 0.100;
                    nextGlobalCooldown = currentTime + 0.0001;
                }
                else
                {
                    double baseGCD = MainWindow.CalcGCD(SPS, MainWindow.GCDLookup, 2.5);
                    nextGlobalCooldown = currentTime + baseGCD;

                }
                currentCast = CastingSpell.None;
                if (debug)
                {
                    MainWindow.log(currentTime.ToString("F3") + "\t-\t" + ability.name + " deals " + thisdamage + critText + " damage.\tMP " + mpbefore + " (-" + manaCost + ") " + '\u2192' + " " + CurrentMP + "\tNext ability at " + nextGlobalCooldown.ToString("F3") + ".\t");
                }
                if (ability.thisSpell == CastingSpell.Fire)
                {
                    int FirestarterRoll = MainWindow.rand.Next(0, 100);
                    if (FirestarterRoll < 40)
                    {
                        if (!firestarter)
                        {
                            firestarter_avail = currentTime + 0.650;
                        }
                        firestarter = true;
                        if (debug)
                        {
                            MainWindow.log(currentTime.ToString("F3") + "\t-\tFirestarter proc!");
                        }
                    }
                }
                switch (ability.thisSpell)
                {
                    case CastingSpell.FireIII: stance = Stance.AF3; break;
                    case CastingSpell.BlizzardIII: stance = Stance.UI3; break;
                    case CastingSpell.FSP: stance = Stance.AF3; break;
                    default: break;
                }
                lastSpell = ability.thisSpell;
                if (ability.debuffTime > 0)
                {
                    bool overwrite = false;
                    foreach (DoT dot in activeDoTs)
                    {
                        if (ability.thisSpell == dot.spell) { overwrite = true; break; }
                    }
                    if (!overwrite)
                    {
                        activeDoTs.Add(new DoT()
                        {
                            name = ability.name,
                            spell = ability.thisSpell,
                            potency = ability.dotPotency,
                            critChance = 5 + ((CRIT - 341) / 13.84598115),
                            damage = damage(ability.dotPotency),
                            fallsOffAt = currentTime + ability.debuffTime,
                            timeLeft = ability.debuffTime
                        });
                        decrement();

                        if (debug)
                        {
                            MainWindow.log(currentTime.ToString("F3") + "\t-\t" + ability.name + " DoT applied.\tFalling off at " + (currentTime + ability.debuffTime).ToString("F3") + ".");
                        }
                    }
                    else
                    {
                        foreach (DoT dot in activeDoTs)
                        {
                            if (ability.thisSpell == dot.spell)
                            {
                                decrement();
                                dot.damage = damage(ability.dotPotency);
                                dot.critChance = 5 + ((CRIT - 341) / 13.84598115);
                                dot.fallsOffAt = currentTime + ability.debuffTime;
                                dot.timeLeft = ability.debuffTime;
                                if (debug)
                                {
                                    MainWindow.log(currentTime.ToString("F3") + "\t-\t" + ability.name + " DoT overwritten.\tFalling off at " + (currentTime + ability.debuffTime).ToString("F3") + ".");
                                }
                                break;
                            }
                        }
                    }
                }
            }

        }
        public override void tick()
        {
            foreach (DoT dot in activeDoTs)
            {
                decrement();
                double critRoll = ((double)MainWindow.rand.Next(0, 1000000) / 10000.0);
                double critChance = 5 + ((CRIT - 341) / 13.84598115);
                string critText = "";
                double critMult = 1.0;
                if (critRoll < critChance)
                {
                    critText = " (Critical!)";
                    totalCrits += 1;
                    critMult = 1.5;
                }
                dot.timeLeft = dot.fallsOffAt - currentTime;
                totalHits += 1;
                totalDamage += (int)Math.Round(dot.damage * critMult);
                totalPotency += (int)dot.potency;

                if (debug)
                {
                    MainWindow.log(currentTime.ToString("F3") + "\t-\t" + dot.name + " is ticking now for " + Math.Round(dot.damage * critMult) + critText + " damage.\tFalling off in " + dot.timeLeft.ToString("F3") + ".");
                }
                if (dot.spell == CastingSpell.Thunder)
                {
                    int tcpRoll = MainWindow.rand.Next(0, 100);
                    if (tcpRoll < 5)
                    {
                        thundercloud = true;
                        if (debug)
                        {
                            MainWindow.log(currentTime.ToString("F3") + "\t-\tThundercloud Proc!");
                        }
                    }
                }
            }
        }
        public override void decrement()
        {
            List<double> values = new List<double>();
            foreach (DoT dot in activeDoTs)
            {
                values.Add(dot.fallsOffAt);
            }
            foreach (Buff buff in activeBuffs)
            {
                values.Add(buff.fallsOffAt);
            }
            if (values.Count > 0)
            {
                nextFallOff = values.Min();
            }
        }
        public override void regen()
        {
            double manaMult = 0.00;
            switch (stance)
            {
                case Stance.UI1: manaMult = 0.30; break;
                case Stance.UI2: manaMult = 0.45; break;
                case Stance.UI3: manaMult = 0.60; break;
                case Stance.None: manaMult = 0.02; break;
                default: manaMult = 0.0; break;
            }
            double HPBefore = (double)CurrentHP;
            double HPgained = Math.Floor((double)MaxHP * 0.01);
            double HPAfter = 0.0;
            if ((int)Math.Floor(HPBefore + HPgained) < MaxHP)
            {
                HPAfter = Math.Floor(HPBefore + HPgained);
            }
            else
            {
                HPAfter = (double)MaxHP;
                HPgained = HPAfter - HPBefore;
            }
            CurrentHP = (int)Math.Floor(HPAfter);
            double MPBefore = (double)CurrentMP;
            double MPgained = Math.Floor((double)MaxMP * manaMult);

            double MPAfter = 0.0;
            if ((int)Math.Floor(MPBefore + MPgained) < MaxMP)
            {
                MPAfter = Math.Floor(MPBefore + MPgained);
            }
            else
            {
                MPAfter = (double)MaxMP;
                MPgained = MPAfter - MPBefore;
            }
            CurrentMP = (int)Math.Floor(MPAfter);
            double TPBefore = (double)CurrentTP;
            double TPgained = 60;
            double TPAfter = 0.0;
            if ((int)Math.Floor(TPBefore + TPgained) < MaxTP)
            {
                TPAfter = Math.Floor(TPBefore + TPgained);
            }
            else
            {
                TPAfter = (double)MaxTP;
                TPgained = TPAfter - TPBefore;
            }
            CurrentTP = (int)Math.Floor(TPAfter);

            HPGained += (int)HPgained;
            MPGained += (int)MPgained;
            TPGained += (int)TPgained;
            if (debug)
            {
                MainWindow.log(currentTime.ToString("F3") + "\t-\tHP " + HPBefore + " (+" + HPgained + ") " + '\u2192' + " " + CurrentHP + "\tMP " + MPBefore + " (+" + MPgained + ") " + '\u2192' + " " + CurrentMP + "\tTP " + TPBefore + " (+" + TPgained + ") " + '\u2192' + " " + CurrentTP);
            }
        }
        public int damage(double pot, bool dot = false)
        {
            double damageformula = 0.0;
            double dpp = (0.000000377653 * (INT * INT) + 0.000000457771 * (DTR - 202.0) * (DTR - 202.0) + 0.0000375062 * MDMG * INT + 0.00000868613 * MDMG * (DTR - 202.0) + 0.000000436208 * INT * (DTR - 202.0) + 0.00226708 * MDMG + 0.000564655 * INT + 0.105144575);
            damageformula = pot * dpp * 1.3;
            return (int)damageformula;
        }

        // -------------------
        // Ability Definition
        // -------------------

        public override void report()
        {
            base.report();
            // add abilities to list used for reporting. Each ability needs to be added 
            abilityReport.Add(ragingstrikes);
            abilityReport.Add(flare);
            abilityReport.Add(swiftcast);
            abilityReport.Add(convert);
            abilityReport.Add(fireiii);
            abilityReport.Add(fireiiifsp);
            abilityReport.Add(fire);
            abilityReport.Add(blizzardiii);
            abilityReport.Add(thunder);
            abilityReport.Add(thunderii);
            abilityReport.Add(thunderiii);
            abilityReport.Add(blizzard);
            abilityReport.Add(thunderiiitcp);
            abilityReport.Add(transpose);

            /*
            if (MainWindow.selenebuff)
            {
                areport.Add(feylight);
                areport.Add(feyglow);
            }
             */
        }

        Ability ragingstrikes = new RagingStrikes();
        Ability blizzard = new Blizzard();
        Ability fireiii = new FireIII();
        Ability fireiiifsp = new FireIIIfsp();
        Ability fire = new Fire();
        Ability blizzardiii = new BlizzardIII();
        Ability thunder = new Thunder();
        Ability thunderii = new ThunderII();
        Ability thunderiii = new ThunderIII();
        Ability thunderiiitcp = new ThunderIIItcp();
        Ability flare = new Flare();
        Ability swiftcast = new Swiftcast();
        Ability convert = new Convert();
        Ability transpose = new Transpose();

        /*
        Ability xpotionintelligence = new XPotionIntelligence();
        Ability feylight = new FeyLight();
        Ability feyglow = new FeyGlow();
        Ability regen = new Regen();
         */


        // Set array of abilities for reportingz

        // -------------------
        // Ability Definition
        // -------------------

        // Weaponskill 1  ---------------------
        public class FireIII : Ability
        {
            public FireIII()
            {
                name = "Fire III";
                abilityType = "Spell";
                castTime = 3.50;
                potency = 240;
                MPcost = 532;
                aspect = Aspect.Fire;
                recastTime = 2.5;
                thisSpell = CastingSpell.FireIII;
                aType = SimXIV.abilityType.Spell;
            }
        }

        public class Fire : Ability
        {
            public Fire()
            {
                name = "Fire";
                abilityType = "Spell";
                castTime = 2.5;
                potency = 170;
                MPcost = 319;
                aspect = Aspect.Fire;
                recastTime = 2.5;
                thisSpell = CastingSpell.Fire;
                aType = SimXIV.abilityType.Spell;
            }
        }

        public class Blizzard : Ability
        {
            public Blizzard()
            {
                name = "Blizzard";
                abilityType = "Spell";
                castTime = 2.5;
                potency = 170;
                MPcost = 106;
                aspect = Aspect.Ice;
                recastTime = 2.5;
                thisSpell = CastingSpell.Blizzard;
                aType = SimXIV.abilityType.Spell;
            }
        }

        public class FireIIIfsp : Ability
        {
            public FireIIIfsp()
            {
                name = "Fire III (FSP)";
                abilityType = "Spell";
                castTime = 0;
                potency = 240;
                MPcost = 0;
                aspect = Aspect.Fire;
                recastTime = 2.5;
                thisSpell = CastingSpell.FSP;
                aType = SimXIV.abilityType.Spell;

            }
        }

        public class BlizzardIII : Ability
        {
            public BlizzardIII()
            {
                name = "Blizzard III";
                abilityType = "Spell";
                castTime = 3.50;
                potency = 240;
                MPcost = 319;
                aspect = Aspect.Ice;
                recastTime = 2.5;
                thisSpell = CastingSpell.BlizzardIII;
                aType = SimXIV.abilityType.Spell;
            }
        }

        public class Thunder : Ability
        {
            public Thunder()
            {
                name = "Thunder";
                abilityType = "Spell";
                castTime = 2.50;
                potency = 30;
                dotPotency = 35;
                debuffTime = 18;
                MPcost = 212;
                aspect = Aspect.Lightning;
                thisSpell = CastingSpell.Thunder;
                aType = SimXIV.abilityType.Spell;
            }
        }

        public class ThunderII : Ability
        {
            public ThunderII()
            {
                name = "Thunder II";
                abilityType = "Spell";
                castTime = 3.00;
                potency = 50;
                dotPotency = 35;
                debuffTime = 21;
                MPcost = 319;
                aspect = Aspect.Lightning;

            }
        }

        public class ThunderIII : Ability
        {
            public ThunderIII()
            {
                name = "Thunder III";
                abilityType = "Spell";
                castTime = 3.50;
                potency = 60;
                dotPotency = 35;
                debuffTime = 24;
                MPcost = 425;
                aspect = Aspect.Lightning;

            }
        }

        public class ThunderIIItcp : Ability
        {
            public ThunderIIItcp()
            {
                name = "Thunder III (TCP)";
                abilityType = "Spell";
                castTime = 0;
                potency = 340;
                dotPotency = 35;
                debuffTime = 24;
                MPcost = 0;
                aspect = Aspect.Lightning;
                thisSpell = CastingSpell.Thunder;
                aType = SimXIV.abilityType.Spell;
            }
        }

        public class Flare : Ability
        {
            public Flare()
            {
                name = "Flare";
                abilityType = "Spell";
                castTime = 4.00;
                potency = 260;
                MPcost = 0;
                aspect = Aspect.Fire;
                recastTime = 2.5;
            }
        }

        public class Swiftcast : Ability
        {
            public Swiftcast()
            {
                name = "Swiftcast";
                abilityType = "Cooldown";
                recastTime = 60;
                animationDelay = 0.8;
            }
        }

        public class Convert : Ability
        {
            public Convert()
            {
                name = "Convert";
                abilityType = "Cooldown";
                recastTime = 180;
                animationDelay = 0.8;
            }
        }

        public class RagingStrikes : Ability
        {
            public RagingStrikes()
            {
                name = "Raging Strikes";
                abilityType = "Cooldown";
                recastTime = 180;
                animationDelay = 0.08;
                buff = 20;
            }
        }

        public class Transpose : Ability
        {
            public Transpose()
            {
                name = "Transpose";
                abilityType = "Cooldown";
                recastTime = 12;
                animationDelay = 0;
                aType = SimXIV.abilityType.Instant;
                thisSpell = CastingSpell.Transpose;
            }
        }
    }
}
