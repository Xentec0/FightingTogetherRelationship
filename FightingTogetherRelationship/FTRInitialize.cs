using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FightingTogetherRelationship
{
    public class FTRInitialize
    {
        //Initializes the values contained in the config xml
        public bool EnableCompanions { get; set; } = true;
        public int CompanionBaseValue { get; set; } = 0;
        public bool EnableSpouse { get; set; } = true;
        public int SpouseBaseValue { get; set; } = 0;
        public bool EnableChildren { get; set; } = true;
        public int ChildrenBaseValue { get; set; } = 0;
        public bool EnableCustomValues { get; set; } = false;
        public int MinimumEnemy { get; set; } = 50;
        public int MinimumAlly { get; set; } = 30;
        public int RelationCap { get; set; } = 100;
        public bool EnableRelationshipLoss { get; set; } = true;
        public double LossMultiplier { get; set; } = 1.0;
        public bool EnableRelationshipGain { get; set; } = true;
        public double GainMultiplier { get; set; } = 1.0;
        public bool EnableChatLogOutput { get; set; } = false;
    }
}
