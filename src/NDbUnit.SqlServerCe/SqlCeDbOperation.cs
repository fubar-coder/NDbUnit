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

using System.Data.Common;
using System.Data.SqlServerCe;
using System.Data;

namespace NDbUnit.Core.SqlServerCe
{
    public class SqlCeDbOperation : DbOperation
    {
        public override string QuotePrefix
        {
            get { return "["; }
        }

        public override string QuoteSuffix
        {
            get { return "]"; }
        }

        protected override DbDataAdapter CreateDbDataAdapter()
        {
            return new SqlCeDataAdapter();
        }

        protected override DbCommand CreateDbCommand(string cmdText)
        {
            return new SqlCeCommand(cmdText);
        }

        protected override void OnRefresh(DataSet ds, IDbCommandBuilder dbCommandBuilder, DbTransaction dbTransaction, string tableName, bool insertIdentity)
        {
            base.OnRefresh(ds, dbCommandBuilder, dbTransaction, tableName, true);
        }
    }
}
