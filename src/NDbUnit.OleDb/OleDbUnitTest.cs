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

using System;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;

namespace NDbUnit.Core.OleDb
{
    /// <summary>
    /// The OleDb unit test data adapter.
    /// </summary>
    /// <example>
    /// <code>
    /// string connectionString = "Provider=SQLOLEDB;Data Source=V-AL-DIMEOLA\NETSDK;Initial Catalog=testdb;Integrated Security=SSPI;";
    /// OleDbUnitTest oleDbUnitTest = new OleDbUnitTest(connectionString);
    /// string xmlSchemaFile = "User.xsd";
    /// string xmlFile = "User.xml";
    ///	oleDbUnitTest.ReadXmlSchema(xmlSchemaFile);
    ///	oleDbUnitTest.ReadXml(xmlFile);
    ///	oleDbUnitTest.PerformDbOperation(DbOperation.CleanInsertIdentity);
    /// </code>
    /// <seealso cref="INDbUnitTest"/>
    /// </example>
    public class OleDbUnitTest : NDbUnitTest<OleDbConnection>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OleDbUnitTest"/> class.
        /// </summary>
        /// <param name="connection">The database connection (<seealso cref="DbConnection"/>)</param>

        public OleDbUnitTest(OleDbConnection connection)
            : base(connection)
        {
            var csb = new OleDbConnectionStringBuilder(connection.ConnectionString);
            ParseConnectionStringBuilder(csb);
        }

        public OleDbUnitTest(string connectionString)
            : base(connectionString)
        {
            var csb = new OleDbConnectionStringBuilder(connectionString);
            ParseConnectionStringBuilder(csb);
        }

        /// <summary>
        /// Gets or sets the OLE database type.  The default value for an 
        /// instance of an object is <see cref="OleDb.OleDbType.NoDb" />.
        /// </summary>
        public OleDbType OleOleDbType
        {
            get
            {
                var op = OleDbOperation;
                if (op != null)
                    return op.OleOleDbType;
                return OleDbType.NoDb;
            }

            set
            {
                var op = OleDbOperation;
                if (op != null)
                    op.OleOleDbType = value;
            }
        }

        private OleDbOperation OleDbOperation
        {
            get { return GetDbOperation() as OleDbOperation; }
        }

        protected override DbDataAdapter CreateDataAdapter(DbCommand command)
        {
            return new OleDbDataAdapter((OleDbCommand)command);
        }

        protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<OleDbConnection> connectionManager)
        {
            return new OleDbCommandBuilder(connectionManager);
        }

        protected override IDbOperation CreateDbOperation()
        {
            return new OleDbOperation();
        }

        private void ParseConnectionStringBuilder(OleDbConnectionStringBuilder csb)
        {
            var provider = csb.Provider.ToLowerInvariant();
            if (string.Equals(provider, "SQLOLEDB", StringComparison.OrdinalIgnoreCase))
            {
                OleOleDbType = OleDbType.SqlServer;
            }
            else if (provider.StartsWith("SQLNCLI", StringComparison.OrdinalIgnoreCase))
            {
                OleOleDbType = OleDbType.SqlServer;
            }
        }
    }
}
