/*
 *
 * NDbUnit
 * Copyright (C) 2005 - 2015
 * https://github.com/NDbUnit/NDbUnit
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

using System.Data;
using System.Data.Common;
using System.IO;

namespace NDbUnit.Core
{
    public interface IDbCommandBuilder
    {
        int CommandTimeOutSeconds { get; set; }
        string QuotePrefix
        {
            get;
        }

        string QuoteSuffix
        {
            get;
        }

        DbConnection Connection
        {
            get;
        }

        void ReleaseConnection();
        DataSet GetSchema();
        void BuildCommands(string xmlSchemaFile);
        void BuildCommands(Stream xmlSchema);
        DbCommand GetSelectCommand(DbTransaction transaction, string tableName);
        DbCommand GetInsertCommand(DbTransaction transaction, string tableName);
        DbCommand GetInsertIdentityCommand(DbTransaction transaction, string tableName);
        DbCommand GetDeleteCommand(DbTransaction transaction, string tableName);
        DbCommand GetDeleteAllCommand(DbTransaction transaction, string tableName);
        DbCommand GetUpdateCommand(DbTransaction transaction, string tableName);
    }
}