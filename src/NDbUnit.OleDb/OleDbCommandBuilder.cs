/*
 *
 * NDbUnit
 * Copyright (C) 2005 - 2015
 * https://github.com/fubar-coder/NDbUnit
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 *
 */

using System.Data.Common;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.Collections;

namespace NDbUnit.Core.OleDb
{
    public class OleDbCommandBuilder : DbCommandBuilder<OleDbConnection>
    {
        public OleDbCommandBuilder(DbConnectionManager<OleDbConnection> connectionManager)
            : base(connectionManager)
        { }

        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override DbCommand CreateDbCommand()
        {
            OleDbCommand command = new OleDbCommand();

            if (CommandTimeOutSeconds != 0)
                command.CommandTimeout = CommandTimeOutSeconds;

            return command;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            return new OleDbParameter("@p" + index, (System.Data.OleDb.OleDbType)dataRow["ProviderType"],
                                      (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
        }

        protected override DbCommand CreateUpdateCommand(DbTransaction transaction, DbCommand selectCommand, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("UPDATE " + TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix) + " SET ");

            OleDbCommand oleDbUpdateCommand = CreateDbCommand() as OleDbCommand;

            int count = 1;
            bool notFirstKey = false;
            bool notFirstColumn = false;
            DbParameter oleDbParameter;
            StringBuilder sbPrimaryKey = new StringBuilder();
            ArrayList keyParameters = new ArrayList();

            bool containsAllPrimaryKeys = true;
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (!(bool)dataRow["IsKey"])
                {
                    containsAllPrimaryKeys = false;
                    break;
                }
            }

            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (ColumnOKToInclude(dataRow))
                {
                    // A key column.
                    if ((bool)dataRow["IsKey"])
                    {
                        if (notFirstKey)
                        {
                            sbPrimaryKey.Append(" AND ");
                        }

                        notFirstKey = true;

                        sbPrimaryKey.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                        sbPrimaryKey.Append("=?");

                        oleDbParameter = (OleDbParameter)CreateNewSqlParameter(count, dataRow);
                        keyParameters.Add(oleDbParameter);

                        ++count;
                    }


                    if (containsAllPrimaryKeys || !(bool)dataRow["IsKey"])
                    {
                        if (notFirstColumn)
                        {
                            sb.Append(", ");
                        }

                        notFirstColumn = true;

                        sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                        sb.Append("=?");

                        oleDbParameter = (OleDbParameter)CreateNewSqlParameter(count, dataRow);
                        oleDbUpdateCommand.Parameters.Add(oleDbParameter);

                        ++count;
                    }
                }
            }

            // Add key parameters last since ordering is important.
            for (int i = 0; i < keyParameters.Count; ++i)
            {
                oleDbUpdateCommand.Parameters.Add((OleDbParameter)keyParameters[i]);
            }

            sb.Append(" WHERE " + sbPrimaryKey);

            oleDbUpdateCommand.CommandText = sb.ToString();

            return oleDbUpdateCommand;
        }

        protected override DbConnection GetConnection(string connectionString)
        {
            return new OleDbConnection(connectionString);
        }

        protected override string GetIdentityColumnDesignator()
        {
            return "IsAutoIncrement";
        }

        protected override string GetParameterDesignator(int count)
        {
            return "?";
        }

    }
}
