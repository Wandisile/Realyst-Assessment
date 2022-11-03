using Microsoft.Extensions.Configuration;
using Microsoft.Office.Interop.Excel;
using Microsoft.Extensions.Hosting;
using BulkUpload;
using System.IO;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;

internal class Program
{
    private static void Main(string[] args)
    {
        var startup = new Startup();
        System.Data.DataTable dt = new System.Data.DataTable();

		try
		{
            string excelConnectionStr = startup.AppSettings.ExcelConnectionStr;
            using (OleDbConnection connExcel = new OleDbConnection(excelConnectionStr))
            {
                using (OleDbCommand cmdExcel = new OleDbCommand())
                {
                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                    {
                        cmdExcel.Connection = connExcel;

                        connExcel.Open();
                        System.Data.DataTable dtExcelSchema;
                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                        connExcel.Close();

                        connExcel.Open();
                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                        odaExcel.SelectCommand = cmdExcel;
                        odaExcel.Fill(dt);
                        connExcel.Close();
                    }
                }

                var dbConnectionStr = startup.AppSettings.DbConnectionStr;
                using (SqlConnection con = new SqlConnection(dbConnectionStr))
                {
                    using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                    {
                        sqlBulkCopy.DestinationTableName = "dbo.Products";

                        sqlBulkCopy.ColumnMappings.Add("Name", "Name");
                        sqlBulkCopy.ColumnMappings.Add("Price", "Price");
                        sqlBulkCopy.ColumnMappings.Add("ReleaseDate", "ReleaseDate");

                        con.Open();
                        sqlBulkCopy.WriteToServer(dt);
                        con.Close();
                    }
                }
                Console.WriteLine("Successfully uploaded data!");
            }
        }
		catch (Exception ex)
		{
            Console.WriteLine(ex.Message);
		}
    }
}
