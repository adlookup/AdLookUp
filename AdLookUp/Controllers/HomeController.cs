using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using AdLookUp.Models;

namespace AdLookUp.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            List<Directory> dict = new List<Directory>();

            using (Domain dom = Domain.GetCurrentDomain())
            {
                using (DirectoryEntry entry = dom.GetDirectoryEntry())
                {
                    using (DirectorySearcher search = new DirectorySearcher(
                        entry, "(&(objectCategory=person)(objectClass=user))"))
                    {
                        search.SearchScope = SearchScope.Subtree;
                        foreach (SearchResult result in search.FindAll())
                        {
                            DirectoryEntry user = result.GetDirectoryEntry();

                            string dn = user.Properties["distinguishedName"].Value.ToString();
                            var dnArray = dn.Split(',').ToList();
                            dnArray.RemoveAt(0);
                            string dirName = String.Join(
                                "/",
                                dnArray
                                    .Where(x => x.StartsWith("CN") || x.StartsWith("OU"))
                                    .Select(o => o.Substring(3)));
                            Directory d = dict.Where(x => x.Name == dirName).FirstOrDefault();
                            if (d == null)
                            {
                                dict.Add(d = new Directory { Name = dirName });
                            }
                            d.Users.Add(new Models.User
                            {
                                Name = user.Name,
                                DistinguishedName = dn
                            });
                        }
                    }
                }
            }


            return View(dict);
        } 
    }
}