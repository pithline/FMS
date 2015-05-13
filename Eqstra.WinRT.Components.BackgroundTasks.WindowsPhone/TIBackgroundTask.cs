﻿
using Eqstra.BusinessLogic.Portable;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Factories;
using Eqstra.TechnicalInspection.UILogic.WindowsPhone.Services;
using Newtonsoft.Json;
using System;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.Storage;
using Windows.UI.Notifications;
using System.Linq;
namespace Eqstra.WinRT.Components.BackgroundTasks.WindowsPhone
{
    public sealed class TIBackgroundTask : IBackgroundTask
    {
        private ITaskService _taskService;
        private string textElementName = "text";
        private BusinessLogic.Portable.TIModels.UserInfo UserInfo;
        async public void Run(IBackgroundTaskInstance taskInstance)
        {
            _taskService = new TaskService(new HttpFactory());
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            await GetTasksforLiveTile();
            deferral.Complete();
        }

        async private System.Threading.Tasks.Task GetTasksforLiveTile()
        {
            try
            {
                if (ApplicationData.Current.RoamingSettings.Values.ContainsKey(Constants.USERINFO))
                {
                    this.UserInfo = JsonConvert.DeserializeObject<Eqstra.BusinessLogic.Portable.TIModels.UserInfo>(ApplicationData.Current.RoamingSettings.Values[Constants.USERINFO].ToString());
                }

                var allTasks = await this._taskService.GetTasksAsync(this.UserInfo.UserId, this.UserInfo.CompanyId);

                if (allTasks != null)
                {
                    var updaterWide = TileUpdateManager.CreateTileUpdaterForApplication();
                    var updaterSqure = TileUpdateManager.CreateTileUpdaterForApplication();
                    var updaterBadge = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
                    updaterWide.EnableNotificationQueue(true);
                    updaterSqure.EnableNotificationQueue(true);
                    updaterWide.Clear();
                    updaterSqure.Clear();
                    updaterBadge.Clear();
                    int counter = 0;

                    foreach (var item in allTasks)
                    {
                        int index = 0;

                        XmlDocument tileXmlSquare = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWide310x150IconWithBadgeAndText);

                        var bindingElementSquare = (XmlElement)tileXmlSquare.GetElementsByTagName("binding").Item(0);
                        bindingElementSquare.SetAttribute("branding", "name");

                        XmlNodeList tileTextAttributes = tileXmlSquare.GetElementsByTagName("text");

                        XmlDocument tileXmlWide = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquare150x150Text01);

                        var bindingElemtWide = (XmlElement)tileXmlWide.GetElementsByTagName("binding").Item(0);
                        bindingElemtWide.SetAttribute("branding", "name");

                        tileTextAttributes[index].InnerText = item.CustomerName.ToString();
                        tileTextAttributes[++index].InnerText = item.ContactName.ToString();
                        tileTextAttributes[++index].InnerText = DateTime.Parse(item.StatusDueDate).Day.ToString() + " " + DateTime.Parse(item.StatusDueDate).ToString("MMMM");


                        char[] whitespace = new char[] { ' ', '\t' };
                        string[] cutName = item.CustomerName.Split(whitespace);

                        if (cutName != null && cutName.Any())
                        {
                            tileXmlWide.GetElementsByTagName(textElementName)[0].InnerText = cutName[0];

                        }
                        tileXmlWide.GetElementsByTagName(textElementName)[1].InnerText = Environment.NewLine;
                        tileXmlWide.GetElementsByTagName(textElementName)[2].InnerText = item.ContactName;
                        tileXmlWide.GetElementsByTagName(textElementName)[3].InnerText = DateTime.Parse(item.StatusDueDate).Day.ToString() + " " + DateTime.Parse(item.StatusDueDate).ToString("MMMM");

                        updaterWide.Update(new TileNotification(tileXmlWide));

                        if (counter++ > 6)
                        {
                            break;
                        }
                        updaterSqure.Update(new TileNotification(tileXmlSquare));
                    }

                    XmlDocument BadgeXml = BadgeUpdateManager.GetTemplateContent(BadgeTemplateType.BadgeNumber);
                    var badgeAttributes = BadgeXml.GetElementsByTagName("badge");
                    ((XmlElement)badgeAttributes[0]).SetAttribute("value", allTasks.Count.ToString());

                    updaterBadge.Update(new BadgeNotification(BadgeXml));
                }
            }
            catch (Exception)
            {

            }

        }

    }
}

