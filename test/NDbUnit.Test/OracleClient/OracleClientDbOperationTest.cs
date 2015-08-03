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
using NDbUnit.Test.Common;
using NDbUnit.OracleClient;
using System.Data;
using NDbUnit.Core;
using NUnit.Framework;
using Oracle.DataAccess.Client;
using System.Data.Common;

namespace NDbUnit.Test.OracleClient
{
    [Category(TestCategories.OracleTests)]
    public class OracleClientDbOperationTest : DbOperationTestBase
    {
        public override void InsertIdentity_Executes_Without_Exception()
        {
            Assert.IsTrue(true);
        }

        protected override NDbUnit.Core.IDbCommandBuilder GetCommandBuilder()
        {
            return new OracleClientDbCommandBuilder(new DbConnectionManager<OracleConnection>(DbConnections.OracleClientConnectionString));
        }

        protected override NDbUnit.Core.IDbOperation GetDbOperation()
        {
            return new OracleClientDbOperation();
        }

        protected override DbCommand GetResetIdentityColumnsDbCommand(DataTable table, DataColumn column)
        {
            throw new NotSupportedException("GetResetIdentityColumnsDbCommand not supported!");
        }

        protected override string GetXmlFilename()
        {
            return XmlTestFiles.OracleClient.XmlFile;
        }

        protected override string GetXmlModifyFilename()
        {
            return XmlTestFiles.OracleClient.XmlModFile;
        }

        protected override string GetXmlRefeshFilename()
        {
            return XmlTestFiles.OracleClient.XmlRefreshFile;
        }

        protected override string GetXmlSchemaFilename()
        {
            return XmlTestFiles.OracleClient.XmlSchemaFile;
        }

    }
}
