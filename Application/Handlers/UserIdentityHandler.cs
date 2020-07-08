using System.Security.Principal;

namespace Application.Handlers
{
    public class UserIdentityHandler
    {
        public static bool IsUserAdministrator()
        {
            bool isAdmin;
            WindowsIdentity user = null;
            try
            {
                user = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(user);
                isAdmin = principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                isAdmin = false;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }

            return isAdmin;
        }
    }
}