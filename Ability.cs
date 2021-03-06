﻿using System;
using System.Collections.Generic;
namespace SimXIV
{
    public enum CastingSpell { FireIII, Fire, Blizzard, BlizzardIII, Thunder, ThunderII, ThunderIII, Flare, FSP, Transpose,None }

    public partial class Ability
    {
        public Aspect aspect;
        public string name = "null";
        public string abilityType = "null";
        public abilityType aType;
        public int potency = 0;
        public string pet = "null";
        public int combopotency = 0;
        public int dotPotency = 0;
        public int hotPotency = 0;
        public double recastTime = 0;
        public double animationDelay = 0;
        public int MPcost = 0;
        public int TPcost = 0;
        public CastingSpell thisSpell;
        public double castTime = 0;
        public double duration = 0;
        public double nextCast = 0.0;
        public double buff = 0;
        public double debuff = 0;
        public double buffTime = 0;
        public double debuffTime = 0;
        public bool autoa = false;
        public bool casting = false;
        public double endcast = 0;
        public bool pierce = false;
        public bool blunt = false;
        public bool slash = false;
        public bool auraon = false;
        public int crits = 0; //number of times this ability successfully crits (must hit)
        public int swings = 0; //includes every use (includes misses,crits)
        public int hits = 0; //includes number of times successfully hit (crits and normal hits)
        public int misses = 0; //number of times ability missed
        public int procs = 0;
        public int ticks = 0; //total number of times dot ticks (crits or normal)
        public int tickcrits = 0;
        public double damage = 0;
        public double heals = 0;
        public double dotdamage = 0;
        public double hotheals = 0;
        public double hot = 0;
        public double hotTime = 0;

        public double dpet = 0.0;
        public double hpet = 0.0;

        
        //public bool dot = false; TODO: add this w/ smn.
        // Dots using dictionary lookup. easyyy.
        public Dictionary<String, Boolean> dotbuff = new Dictionary<String, Boolean>() {
        {"ragingstrikes",false},
        {"bloodforblood",false},
        {"straightshot",false},
        {"hawkseye",false},
        {"internalrelease",false},
        {"fightorflight",false},
        {"cooldown1",false},
        {"heavythrust",false},
        {"potion", false},
        {"song", false}
    };

        public double bonus = 0; // for abilitiy specific bonuses and potions
        public double percent = 0; // for things that increase by percentage.
        public void resetAbility()
        {
            this.crits = 0;
            this.swings = 0;
            this.hits = 0;
            this.misses = 0;
            this.procs = 0;
            this.ticks = 0;
            this.tickcrits = 0;
            this.nextCast = 0.0;
            this.damage = 0;
            this.heals = 0;
            this.dotdamage = 0;
            this.hotheals = 0;
            this.hot = 0;
            this.buff = 0;
            //this.totalattacks = 0;
            //this.totaldotticks = 0;
            this.dpet = 0;
            this.hpet = 0;
        }
        //public double calculateCrit(Job _player) { return (0.0693 * _player.CRIT - 18.486); }



    }
    public enum Aspect { Fire, Ice, Lightning, Water, Earth, Wind, None }
    public enum abilityType { Spell, Instant, None }

}


