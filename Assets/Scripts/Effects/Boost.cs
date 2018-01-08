
using System.Collections.Generic;

namespace Effects
{
    public class Boost
    {
        public static List<Boost> allBoosts = new List<Boost>()
        {
            new Boost("Dream catcher", "Extands parts collection radius", 
                300, BoostType.Magnet, 1),
            new Boost("BF Magnet", "Catch goods from the all 3 lines!", 
                1000, BoostType.Magnet, 2),
            new Boost("9 lives", "If you crash your jet or lose your speed, you get 1 extra chance to get back on track", 
                600, BoostType.Defence, 1),
            new Boost("Leakproof gas tank", "You can dash longer", 
                600, BoostType.Fuel, 1),
            new Boost("Nitro boost", "You prepare for the next dash faster", 
                200, BoostType.SpeedUp, 1),
            new Boost("Engine stabilizer", "Your initial speed is slower than increases over time", 
                300, BoostType.SpeedDown, 1),
            new Boost("Shoot 'em up", "You can shoot faster", 1200, BoostType.Gun, 1)
        };
        
        public enum BoostType
        {
            Magnet, SpeedUp, SpeedDown, Gun, Defence, Fuel
        }
        
        private string _name;
        private string _description;
        private int _cost;
        private BoostType _type;
        private int _level;

        public Boost(string name, string description, int cost, BoostType type, int level)
        {
            _name = name;
            _description = description;
            _cost = cost;
            _type = type;
            _level = level;
        }

        public string GetName()
        {
            return _name;
        }

        public string GetDescription()
        {
            return _description;
        }

        public int GetCost()
        {
            return _cost;
        }

        public BoostType GetBoostType()
        {
            return _type;
        }

        public int GetLevel()
        {
            return _level;
        }
    }
}