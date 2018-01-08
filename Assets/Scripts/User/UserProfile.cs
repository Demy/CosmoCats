namespace User
{
    public class UserProfile
    {
        private int _id;
        private int _points;

        public UserProfile(int id, int points)
        {
            _id = id;
            _points = 1000;//points;
        }

        public int GetId()
        {
            return _id;
        }

        public int GetPoints()
        {
            return _points;
        }

        public void ChangePointsBy(int value)
        {
            _points += value;
        }
    }
}