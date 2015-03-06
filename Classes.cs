using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimXIV
{
    public class BasePlayer
    {
        public JobEnum job;
        public double WDMG;
        public double MDMG;
        public double AADMG;
        public double AADELAY;
        public double STR;
        public double DEX;
        public double VIT;
        public int MaxHP;
        public double INT;
        public double MND;
        public double PIE;
        public int MaxMP;
        public double ACC;
        public double CRIT;
        public double DTR;
        public double SKS;
        public double SPS;
    }

    public enum JobEnum
    {
        BlackMage,
        None
    }

    public class DoT
    {
        public string name;
        public CastingSpell spell;
        public double critChance;
        public double potency;
        public double damage;
        public double fallsOffAt;
        public double timeLeft;
    }

    public class Buff
    {
        public string name;
        public double fallsOffAt;
        public double timeLeft;
    }


}
