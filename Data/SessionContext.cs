namespace GymManagementClient.Data
{
    public static class SessionContext
    {
        public static string CurrentRole { get; set; }
        public static string ConnectionString { get; set; }

        public static void ResetSession()
        {
            CurrentRole = null;
            ConnectionString = null;
        }
    }
}
