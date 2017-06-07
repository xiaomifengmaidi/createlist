using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace CreateList
{
    class Program
    {
        static void Main(string[] args)
        {
            SPSite site = new SPSite("https://portal.shanghaipower.com/sites/km", SPUserToken.SystemAccount);
          //  SPSite site = new SPSite();
            SPWeb web = site.RootWeb;
          SPWeb webtest = site.OpenWeb("test");
           // SPWeb webcontract = site.OpenWeb("contract/contract");
            //SPWeb webproject = site.OpenWeb("project");
            //SPWeb webfinance = site.OpenWeb("finance");
            //SPWeb webdevice = site.OpenWeb("device");
            //SPWeb webmaterial = site.OpenWeb("material");
            //SPWeb webperformance = site.OpenWeb("performance");
            SPList listAD = web.Lists["11"];
            SPListItemCollection items = listAD.GetItems();
            //SPList listUser= web.Lists["用户"];
            //SPListItemCollection itemsUser = listUser.GetItems();
            foreach (SPListItem item in items)
            {

                SPFieldUserValue userValue = new SPFieldUserValue(web, item["用户组"] + "");

              // SPGroup userGroup = web.SiteGroups.GetByID(userValue.LookupId);
                SetFilePermission(webtest, getDocumentLibrary(webtest, item["Title"] + ""), userValue.User);
               // SetFilePermission(webcontract, getDocumentLibrary(webcontract, item["Title"] + ""), userValue.User);
                //SetFilePermission(webproject, getDocumentLibrary(webproject, item["Title"] + ""), userValue.User);
                //SetFilePermission(webfinance, getDocumentLibrary(webfinance, item["Title"] + ""), userValue.User);
                //SetFilePermission(webdevice, getDocumentLibrary(webdevice, item["Title"] + ""), userValue.User);
                //SetFilePermission(webmaterial, getDocumentLibrary(webmaterial, item["Title"] + ""), userValue.User);
                //SetFilePermission(webperformance, getDocumentLibrary(webperformance, item["Title"] + ""), userValue.User);

            }
            //foreach (SPListItem item in items)
            //{

            //    SPFieldUserValue userValue = new SPFieldUserValue(web, itemsUser[0]["username"]+"");
            //    SetFilePermission(webfinance, getDocumentLibrary(webfinance, item["Title"] + ""), userValue.User);


            //}
        }
        private static SPDocumentLibrary getDocumentLibrary(SPWeb web,string listName)
        {
            SPDocumentLibrary library = web.Lists.TryGetList(listName) as SPDocumentLibrary;
            if (library == null)
            {
                SPListTemplateCollection listTemplates = web.Site.GetCustomListTemplates(web);
                SPListTemplate listTemplate = listTemplates["SECPKM_Library"];
                Guid libraryUid = web.Lists.Add(listName, listName, listTemplate);
                library = web.Lists[libraryUid] as SPDocumentLibrary;
            }
            return library;
        }
        public static void SetFilePermission(SPWeb web,SPDocumentLibrary lib,SPUser group)
        {
            //改变站点继承权
            if (!web.HasUniqueRoleDefinitions)
            {
                // web.RoleDefinitions.BreakInheritance(false, false);//复制父站点角色定义并且保持权限
            }

            //站点继承权改变后重新设置状态
            web.AllowUnsafeUpdates = true;
            lib.BreakRoleInheritance(false, false);
            SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
            SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
            roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
            lib.RoleAssignments.Add(roleAssignment);
        }
    }
}
