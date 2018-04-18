#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;

using FluentMigrator.Expressions;

using JetBrains.Annotations;

using Microsoft.Extensions.DependencyInjection;

namespace FluentMigrator.Infrastructure
{
    /// <summary>
    /// The default implementation of the <see cref="IMigrationContext"/>
    /// </summary>
    public class MigrationContext : IMigrationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationContext"/> class.
        /// </summary>
        /// <param name="querySchema">The provider used to query the database</param>
        /// <param name="migrationAssemblies">The collection of migration assemblies</param>
        /// <param name="context">The arbitrary application context passed to the task runner</param>
        /// <param name="connection">The database connection</param>
        [Obsolete]
        public MigrationContext([NotNull] IQuerySchema querySchema, [NotNull] IAssemblyCollection migrationAssemblies, object context, string connection)
        {
            Expressions = new List<IMigrationExpression>();
            QuerySchema = querySchema;
            MigrationAssemblies = migrationAssemblies;
            ApplicationContext = context;
            Connection = connection;
            var services = new ServiceCollection();
            services
                .AddScoped(sp => migrationAssemblies)
                .AddScoped<IEmbeddedResourceProvider, DefaultEmbeddedResourceProvider>();
            ServiceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationContext"/> class.
        /// </summary>
        /// <param name="querySchema">The provider used to query the database</param>
        /// <param name="context">The arbitrary application context passed to the task runner</param>
        /// <param name="connection">The database connection</param>
        /// <param name="serviceProvider">The service provider</param>
        public MigrationContext([NotNull] IQuerySchema querySchema, object context, string connection, [NotNull] IServiceProvider serviceProvider)
        {
            Expressions = new List<IMigrationExpression>();
            QuerySchema = querySchema;
#pragma warning disable 612
            MigrationAssemblies = serviceProvider?.GetService<IAssemblyCollection>();
#pragma warning restore 612
            ApplicationContext = context;
            Connection = connection;
            ServiceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public virtual ICollection<IMigrationExpression> Expressions { get; set; }

        /// <inheritdoc />
        public virtual IQuerySchema QuerySchema { get; set; }

        /// <inheritdoc />
        [Obsolete]
        [CanBeNull]
        public virtual IAssemblyCollection MigrationAssemblies { get; set; }

        /// <inheritdoc />
        public virtual object ApplicationContext { get; set; }

        /// <inheritdoc />
        public string Connection { get; set; }

        /// <inheritdoc />
        [NotNull]
        public IServiceProvider ServiceProvider { get; }
    }
}
