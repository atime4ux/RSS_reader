using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace libRSSreader
{
    public class clsCmnDB
    {
        libCommon.clsDB objDB = new libCommon.clsDB();
        libCommon.clsUtil objUtil = new libCommon.clsUtil();

        public string INSERT_DB(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string tbName, string cols, string vals)
        {
            int i;

            string Result = "";

            StringBuilder strBuilder = new StringBuilder();

            System.Collections.ArrayList arrCols = new System.Collections.ArrayList();
            System.Collections.ArrayList arrVals = new System.Collections.ArrayList();

            arrCols.AddRange(objUtil.Split(cols, "|"));
            arrVals.AddRange(objUtil.Split(vals, "|"));

            strBuilder.Append("INSERT INTO " + tbName);
            strBuilder.Append(" (");
            for (i = 0; i < arrCols.Count; i++)
            {
                if (i > 0)
                {
                    strBuilder.Append(", ");
                }
                strBuilder.Append(objUtil.toDb(arrCols[i].ToString()));

            }
            strBuilder.Append(") VALUES (");
            for (i = 0; i < arrVals.Count; i++)
            {
                if (i > 0)
                {
                    strBuilder.Append(", ");
                }
                strBuilder.Append("'");
                strBuilder.Append(objUtil.toDb(arrVals[i].ToString().Replace("%/","|")));
                strBuilder.Append("'");

            }
            strBuilder.Append(")");

            objUtil.writeLog("INSERT_DB QUERY : " + strBuilder.ToString());

            try
            {
                Result = objDB.ExecuteNonQuery(dbCon, TRX, strBuilder.ToString());
            }
            catch (Exception e)
            {
                objUtil.writeLog("ERR CMN INSERT [" + tbName + "] " + "[" + cols + "] " + "[" + vals + "]");
                objUtil.writeLog("ERR CMN INSERT QUERY : " + strBuilder.ToString());
                objUtil.writeLog("ERR CMN INSERT MSG : " + e.ToString());
                Result = "FAIL";
            }

            return Result;

        }
        public string UPDATE_DB(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string tbName, string cols, string vals, string Wcols, string Wvals)
        {
            int i;

            string Result = "";

            StringBuilder strBuilder = new StringBuilder();

            System.Collections.ArrayList arrCols = new System.Collections.ArrayList();
            System.Collections.ArrayList arrVals = new System.Collections.ArrayList();
            System.Collections.ArrayList arrWCols = new System.Collections.ArrayList();
            System.Collections.ArrayList arrWVals = new System.Collections.ArrayList();

            arrCols.AddRange(objUtil.Split(cols, "|"));
            arrVals.AddRange(objUtil.Split(vals, "|"));
            arrWCols.AddRange(objUtil.Split(Wcols, "|"));
            arrWVals.AddRange(objUtil.Split(Wvals, "|"));

            strBuilder.Append("UPDATE " + tbName);
            strBuilder.Append(" SET ");

            for (i = 0; i < arrCols.Count; i++)
            {
                if (i > 0)
                {
                    strBuilder.Append(", ");
                }
                strBuilder.Append("[");
                strBuilder.Append(objUtil.toDb(arrCols[i].ToString()));
                strBuilder.Append("]");
                strBuilder.Append(" = ");
                strBuilder.Append("'");
                strBuilder.Append(objUtil.toDb(arrVals[i].ToString()).Replace("%/", "|"));
                strBuilder.Append("'");

            }
            strBuilder.Append(" WHERE ");
            for (i = 0; i < arrWCols.Count; i++)
            {
                if (i > 0)
                {
                    strBuilder.Append(" AND ");
                }
                strBuilder.Append("[");
                strBuilder.Append(objUtil.toDb(arrWCols[i].ToString()));
                strBuilder.Append("]");
                strBuilder.Append(" = ");
                strBuilder.Append("'");
                strBuilder.Append(objUtil.toDb(arrWVals[i].ToString()));
                strBuilder.Append("'");

            }

            objUtil.writeLog("UPDATE_DB QUERY : " + strBuilder.ToString());

            try
            {
                Result = objDB.ExecuteNonQuery(dbCon, TRX, strBuilder.ToString());
            }
            catch (Exception e)
            {
                objUtil.writeLog("ERR CMN UPDATE [" + tbName + "] " + "[" + cols + "] " + "[" + vals + "]");
                objUtil.writeLog("ERR CMN UPDATE QUERY : " + strBuilder.ToString());
                objUtil.writeLog("ERR CMN UPDATE MSG : " + e.ToString());
                Result = "FAIL";
            }

            return Result;
        }
        public string DELETE_DB(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string tbName, string Wcols, string Wvals)
        {
            int i;

            string Result = "";

            StringBuilder strBuilder = new StringBuilder();

            libCommon.clsDB objDB = new libCommon.clsDB();
            libCommon.clsUtil objUtil = new libCommon.clsUtil();

            System.Collections.ArrayList arrWCols = new System.Collections.ArrayList();
            System.Collections.ArrayList arrWVals = new System.Collections.ArrayList();

            arrWCols.AddRange(objUtil.Split(Wcols, "|"));
            arrWVals.AddRange(objUtil.Split(Wvals, "|"));

            strBuilder.Append("DELETE FROM " + tbName);
            strBuilder.Append(" WHERE ");
            for (i = 0; i < arrWCols.Count; i++)
            {
                if (i > 0)
                {
                    strBuilder.Append(" AND ");
                }
                strBuilder.Append("[");
                strBuilder.Append(objUtil.toDb(arrWCols[i].ToString()));
                strBuilder.Append("]");
                strBuilder.Append(" = ");
                strBuilder.Append("'");
                strBuilder.Append(objUtil.toDb(arrWVals[i].ToString()));
                strBuilder.Append("'");
            }

            try
            {
                Result = objDB.ExecuteNonQuery(dbCon, TRX, strBuilder.ToString());
            }
            catch (Exception e)
            {
                objUtil.writeLog("ERR CMN DELETE [" + tbName + "] " + "[" + Wcols + "] " + "[" + Wvals + "]");
                objUtil.writeLog("ERR CMN DELETE QUERY : " + strBuilder.ToString());
                objUtil.writeLog("ERR CMN DELETE MSG : " + e.ToString());
                Result = "FAIL";
            }

            return Result;
        }

        public string getNewID(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string tbName, string kcol, string Wcols, string Wvals, int len)
        {
            System.Data.DataSet DS = new System.Data.DataSet();
            StringBuilder strBuilder = new StringBuilder();
            int i;

            string Result;
            string dKey = "".PadLeft(len, '0');

            System.Collections.ArrayList arrWCols = new System.Collections.ArrayList();
            System.Collections.ArrayList arrWVals = new System.Collections.ArrayList();

            arrWCols.AddRange(objUtil.Split(Wcols, "|"));
            arrWVals.AddRange(objUtil.Split(Wvals, "|"));


            strBuilder.Append("SELECT ISNULL(MAX(" + kcol + "), '" + dKey + "') FROM " + tbName);
            strBuilder.Append(" WHERE 1 = 1");
            for (i = 0; i < arrWCols.Count; i++)
            {
                if (arrWCols[i].ToString().Length > 0)
                {
                    strBuilder.Append(" AND ");
                    strBuilder.Append("[");
                    strBuilder.Append(objUtil.toDb(arrWCols[i].ToString()));
                    strBuilder.Append("]");
                    strBuilder.Append(" = ");
                    strBuilder.Append("'");
                    strBuilder.Append(objUtil.toDb(arrWVals[i].ToString()));
                    strBuilder.Append("'");
                }
            }

            objUtil.writeLog("GET NEW ID QUERY : " + strBuilder.ToString());

            DS = objDB.ExecuteDSQuery(dbCon, TRX, strBuilder.ToString());

            Result = (objUtil.ToInt32(DS.Tables[0].Rows[0][0].ToString()) + 1).ToString().PadLeft(len, '0');

            objUtil.writeLog("GET NEW ID : " + Result);

            return Result;
        }

        /// <summary>
        /// DATATABLE과 DATAROW가 존재하면 true 리턴
        /// </summary>
        public bool validateDS(System.Data.DataSet DS)
        {
            bool Result = false;

            if (DS.Tables.Count > 0)
            {
                if (DS.Tables[0].Rows.Count > 0)
                {
                    Result = true;
                }
            }

            return Result;
        }

        /// <summary>
        /// 중복된 값이 없으면 "OK", 중복된 값이 있으면 "DUPE", 에러는 "FAIL"리턴
        /// </summary>
        public string chk_duplicate(System.Data.SqlClient.SqlConnection dbCon, System.Data.SqlClient.SqlTransaction TRX, string Col, string tbName, string wCols, string wVals)
        {
            System.Text.StringBuilder strBuilder = new StringBuilder();
            System.Data.DataSet DS = new System.Data.DataSet();

            string[] arr_wCols = objUtil.Split(wCols, "|");
            string[] arr_wVals = objUtil.Split(wVals, "|"); ;

            string Result = "FAIL";

            int i;

            strBuilder.Append("SELECT");
            strBuilder.Append(" COUNT(" + Col + ")");
            strBuilder.Append(" FROM " + tbName);
            strBuilder.Append(" WHERE 1=1");

            if (wCols.Length > 0)
            {
                for (i = 0; i < arr_wCols.Length; i++)
                {
                    if (arr_wCols[i].Length > 0)
                    {
                        strBuilder.Append(" AND");
                        strBuilder.Append(" " + arr_wCols[i] + "=");
                        strBuilder.Append("'" + arr_wVals[i] + "'");
                    }
                }
            }

            DS = objDB.ExecuteDSQuery(dbCon, TRX, strBuilder.ToString());

            if (validateDS(DS))
            {
                if (DS.Tables[0].Rows[0][0].ToString().Equals("0"))
                {
                    Result = "OK";
                }
                else
                {
                    Result = "DUPE";
                }
            }
            else
            {
                objUtil.writeLog("ERR : CHECK DUPLICATE");
            }

            return Result;
        }
    }
}
