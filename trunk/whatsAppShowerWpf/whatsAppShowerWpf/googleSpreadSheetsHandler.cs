using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Google.GData.Client;
using Google.GData.Extensions;
using Google.GData.Spreadsheets;
using log4net;

namespace whatsAppShowerWpf
{
     
    class googleSpreadSheetsHandler
    {
        private static readonly ILog systemLog = log4net.LogManager.GetLogger("systemsLog");
        public string isCanLoginIn()
        {
            try
            {
                SpreadsheetsService service = new SpreadsheetsService("whatsappshower");
                service.setUserCredentials("whatsappshower@gmail.com", "e4rst6rh");

                SpreadsheetQuery query = new SpreadsheetQuery();
                SpreadsheetFeed feed = service.Query(query);

                

                SpreadsheetEntry whatsAppShowerCerEntry = null;
                foreach (SpreadsheetEntry entry in feed.Entries)
                {
                    //Console.WriteLine(entry.Title.Text);
                    if ("whatsAppShowerCer".Equals(entry.Title.Text))
                    {
                        whatsAppShowerCerEntry = entry;
                    }

                }
                if (whatsAppShowerCerEntry == null)
                {
                    return null;
                }



                AtomLink link = whatsAppShowerCerEntry.Links.FindService(GDataSpreadsheetsNameTable.WorksheetRel, null);

                WorksheetQuery worksheetQueryQuery = new WorksheetQuery(link.HRef.ToString());
                WorksheetFeed worksheetQueryFeed = service.Query(worksheetQueryQuery);
                WorksheetEntry whatsAppShowerCerWorksheet = null;
                foreach (WorksheetEntry worksheet in worksheetQueryFeed.Entries)
                {
                    if ("1".Equals(worksheet.Title.Text))
                    {
                        whatsAppShowerCerWorksheet = worksheet;
                    }
                }
                if (whatsAppShowerCerWorksheet == null)
                {
                    return null;
                }

                AtomLink cellFeedLink = whatsAppShowerCerWorksheet.Links.FindService(GDataSpreadsheetsNameTable.CellRel, null);

                CellQuery cellQueryQuery = new CellQuery(cellFeedLink.HRef.ToString());
                CellFeed cellQueryFeed = service.Query(cellQueryQuery);

                bool foundToken = false;
                foreach (CellEntry curCell in cellQueryFeed.Entries)
                {
                    if (foundToken)
                    {
                        return curCell.Cell.Value;
                    }
                    if (curCell.Cell.Column == 1)
                    {
                        if (curCell.Cell.Value.Equals(WhatsappProperties.Instance.AppToken))
                        {
                            foundToken = true;
                        }
                    }

                }
            }
            catch (Exception e)
            {
                systemLog.Error(e);
            }

            return null;
        }
    }
}
