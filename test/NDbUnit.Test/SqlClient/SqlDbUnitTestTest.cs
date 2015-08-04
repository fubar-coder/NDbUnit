﻿/*
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

using NDbUnit.Core;
using System.Data;
using System.IO;
using NDbUnit.Core.SqlClient;
using System.Collections.Generic;
using System.Data.SqlClient;
using NUnit.Framework;
using System.Data.Common;

namespace NDbUnit.Test.SqlServerCe
{
    [Category(TestCategories.SqlServerTests)]
    [TestFixture]
    public class SqlDbUnitTestTest : NDbUnit.Test.Common.DbUnitTestTestBase
    {
        public override IList<string> ExpectedDataSetTableNames
        {
            get
            {
                return new List<string>()
                {
                    "Role", "dbo.User", "UserRole" 
                };
            }
        }

        protected override IUnitTestStub GetUnitTestStub()
        {
            return new SqlUnitTestStub(DbConnections.SqlConnectionString);
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.SqlServer.XmlFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.SqlServer.XmlSchemaFile;
        }

        protected class SqlUnitTestStub : SqlDbUnitTest, IUnitTestStub
        {
            public SqlUnitTestStub(string connectionString)
                : base(connectionString)
            {
            }

            protected override IDbCommandBuilder CreateDbCommandBuilder(DbConnectionManager<SqlConnection> connectionManager )
            {
                return _mockDbCommandBuilder;
            }

            protected override IDbOperation CreateDbOperation()
            {
                return _mockDbOperation;
            }

            protected override DbDataAdapter CreateDataAdapter(DbCommand command)
            {
                return base.CreateDataAdapter(command);
            }

            protected override FileStream GetXmlSchemaFileStream(string xmlSchemaFile)
            {
                return _mockSchemaFileStream;
            }

            protected override FileStream GetXmlDataFileStream(string xmlFile)
            {
                return _mockDataFileStream;
            }

            protected override DataSet DS
            {
                get { return base.DS; }
            }

            public DataSet TestDataSet
            {
                get { return DS; }
            }
        }

    }

}

