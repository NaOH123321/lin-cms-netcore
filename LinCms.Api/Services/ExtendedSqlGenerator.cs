using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Pomelo.EntityFrameworkCore.MySql.Migrations;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace LinCms.Api.Services
{
    public class ExtendedMySqlGenerator : MySqlMigrationsSqlGenerator
    {
        public ExtendedMySqlGenerator(MigrationsSqlGeneratorDependencies dependencies, IMigrationsAnnotationProvider migrationsAnnotations, IMySqlOptions options) : base(dependencies, migrationsAnnotations, options)
        {

        }

        protected override void CreateTableForeignKeys(CreateTableOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            
            //base.CreateTableForeignKeys(operation, model, builder);
        }

        protected override void Generate(AddForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder,
            bool terminate = true)
        {
            //base.Generate(operation, model, builder, terminate);
        }

        protected override void Generate(DropForeignKeyOperation operation, IModel model, MigrationCommandListBuilder builder, bool terminate)
        {
            //base.Generate(operation, model, builder, terminate);
        }
    }
}
