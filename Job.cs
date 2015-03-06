using System;
using System.Collections.Generic;

namespace SimXIV
{
    public partial class Job
    {
        //Stats
        public JobEnum JobName;
        public string name;
        public string classname;
        public double WDMG;
        public double MDMG;
        public double AADMG;
        public double AADELAY;
        public double STR;
        public double DEX;
        public double VIT;
        public int MaxHP;
        public int CurrentHP;
        public double INT;
        public double MND;
        public double PIE;
        public int MaxMP;
        public int CurrentMP;
        public int MaxTP;
        public int CurrentTP;
        public double ACC;
        public double CRIT;
        public double DTR;
        public double SKS;
        public double SPS;
        public double lastVitalTick;

        public List<DoT> activeDoTs = new List<DoT>();
        public List<Buff> activeBuffs = new List<Buff>();

        //Time
        public double fightLength; //
        public double currentTime; //
        public double currentDotTick; //
        public double nextVitalTick; //
        public double nextGlobalCooldown;
        public double nextInstant;
        public double nextAutoAttack;
        public double finishCastAt;
        public double nextFallOff;
        public bool isCasting;

        //Records
        public int totalDamage;
        public int totalPotency;
        public int totalCrits;
        public int totalNonCrits;
        public int totalSwings;
        public int totalHits;
        public int totalMisses;
        public int HPGained;
        public int HPUsed;
        public int MPGained;
        public int MPUsed;
        public int TPGained;
        public int TPUsed;

        public bool debug;
        public List<Ability> abilityReport = new List<Ability>();

        public virtual void report()
        {

        }

        public virtual void rotation() 
        { 
            
        }
        public virtual void decrement()
        {

        }
        public virtual void execute(ref Ability ability)
        {

        }
        public virtual void impact(ref Ability ability)
        {

        }
        public virtual void tick()
        {

        }
        public virtual void regen()
        {
            
        }
        public double calculateACC()
        {
            var acccalc = 0.00;
            if (name == "Black Mage") { acccalc = 0.147287 * ACC + 30.775194; }
            if (name == "Bard" || name == "Dragoon" || name == "Monk") { acccalc = 0.12419 * ACC + 38.653595; }
            return acccalc;
        }
    }
}
