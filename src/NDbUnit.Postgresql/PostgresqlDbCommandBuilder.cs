/*
 *
 * NDbUnit
 * Copyright (C) 2005 - 2015
 * https://github.com/NDbUnit/NDbUnit
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 *
 */

using System;
using System.Text;
using System.Data;
using NDbUnit.Core;
using Npgsql;
using NpgsqlTypes;
using System.Data.Common;

namespace NDbUnit.Postgresql
{
    public class PostgresqlDbCommandBuilder : NDbUnit.Core.DbCommandBuilder<NpgsqlConnection>
    {
        public PostgresqlDbCommandBuilder(DbConnectionManager<NpgsqlConnection> connectionManager)
            : base(connectionManager)
        {
        }

        public override string QuotePrefix
        {
            get { return "\""; }
        }

        public override string QuoteSuffix
        {
            get { return QuotePrefix; }
        }

        protected override DbCommand CreateDbCommand()
        {
            NpgsqlCommand command = new NpgsqlCommand();
            return command;
        }

        protected override DbCommand CreateInsertCommand(DbCommand selectCommand, string tableName)
        {
            int count = 1;
            bool notFirstColumn = false;
            StringBuilder sb = new StringBuilder();
            sb.Append(String.Format("INSERT INTO {0}(", TableNameHelper.FormatTableName(tableName, QuotePrefix, QuoteSuffix)));
            StringBuilder sbParam = new StringBuilder();
            IDataParameter sqlParameter;
            DbCommand sqlInsertCommand = CreateDbCommand();
            foreach (DataRow dataRow in _dataTableSchema.Rows)
            {
                if (notFirstColumn)
                {
                    sb.Append(", ");
                    sbParam.Append(", ");
                }

                notFirstColumn = true;

                sb.Append(QuotePrefix + dataRow["ColumnName"] + QuoteSuffix);
                sbParam.Append(GetParameterDesignator(count));

                sqlParameter = CreateNewSqlParameter(count, dataRow);
                sqlInsertCommand.Parameters.Add(sqlParameter);

                ++count;
            }

            sb.Append(String.Format(") VALUES({0})", sbParam));

            sqlInsertCommand.CommandText = sb.ToString();

            return sqlInsertCommand;
        }

        protected override IDataParameter CreateNewSqlParameter(int index, DataRow dataRow)
        {
            //return new NpgsqlParameter("p" + index, (NpgsqlDbType)dataRow["ProviderType"],
            //                          (int)dataRow["ColumnSize"], (string)dataRow["ColumnName"]);
            return new NpgsqlParameter
                       {
                           ParameterName = "p" + index,
                           Size = (int)dataRow["ColumnSize"],
                           SourceColumn = (string)dataRow["ColumnName"]
                       };
        }

        protected override DbConnection GetConnection(string connectionString)
        {
            return new NpgsqlConnection(connectionString);
        }

        protected override string GetIdentityColumnDesignator()
        {
            return "IsAutoIncrement";
        }

        protected override string GetParameterDesignator(int count)
        {
            return ":p" + count;
        }
    }
}
