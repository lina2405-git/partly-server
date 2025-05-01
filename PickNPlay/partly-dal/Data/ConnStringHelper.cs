namespace PickNPlay.picknplay_dal.Data
{
    public class ConnStringHelper
    {
        public static string GetRDSConnectionString()
        {
            string username = "admin";
            string password = "masterpicknplay";
            string hostname = "picknplay-db.cr6wcuacgsku.us-east-1.rds.amazonaws.com";
            string port = "1433";

            return "Data Source=" + hostname + "," + port + ";Initial Catalog=" + "pick-n-play-db" + ";User ID=" + username + ";Password=" + password + ";";
        }
    }
}
