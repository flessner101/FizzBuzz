#region Namespaces
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

#endregion

namespace FizzBuzz
{
    [Transaction(TransactionMode.Manual)]
    public class Command : IExternalCommand
    {
        public Result Execute(
          ExternalCommandData commandData,
          ref string message,
          ElementSet elements)
        {
            UIApplication uiapp = commandData.Application;
            UIDocument uidoc = uiapp.ActiveUIDocument;
            Application app = uiapp.Application;
            Document doc = uidoc.Document;

            int range = 100;
            double offset = 0.05;
            double offsetCalc = offset * doc.ActiveView.Scale;
            string fizzbuzz = "";

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfClass(typeof(TextNoteType));

            XYZ curPoint = new XYZ(0, 0, 0);
            XYZ pointOffset = new XYZ(0, offsetCalc, 0);


            using (Transaction tx = new Transaction(doc))
            {
                tx.Start("Transaction Name");

                // i starts at 1 otherwise we get a "FIZZ" to start the list
                for (int i = 1; i <= 100; i++)
                
                    if (i % 3 != 0 && i % 5 != 0)
                    {
                        fizzbuzz = i.ToString();
                    }
                    else if (i % 3 == 0)
                    {
                        fizzbuzz = "FIZZ";
                    }
                    else if (i % 5 == 0)
                    {
                        fizzbuzz = "BUZZ";
                    }
                    else
                    {
                        fizzbuzz = "FIZZBUZZ";
                    }

                    // Place the Fizz Buzz printed items here 
                    TextNote.Create(doc, doc.ActiveView.Id, curPoint, fizzbuzz, collector.FirstElementId());
                    curPoint = curPoint.Subtract(pointOffset);
                }

                tx.Commit();
            }

            return Result.Succeeded;
        }
    }
}
