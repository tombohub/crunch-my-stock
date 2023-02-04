## Scaffold for Entity Core models. Tables in database are managed by Django Orm.

import os

# configuration
database_name = 'stock_analytics'
models_dir = 'Database/Models/'
context_name = 'stock_analyticsContext'
context_file = context_name + '.cs'
context_dir = models_dir


# delete all the models in Data/Models to remove non existing tables
# if any is deleted or name changed after migration.
for file in os.listdir(models_dir):
    os.remove(models_dir + file)
# also delete context file because build will failed since models are
# referenced in context and now they're gone
try:
    os.remove(context_file)
except Exception:
    print('no context file present')

#NOTE: these tables are from another project
tables_to_scaffold = ['prices', 'stores', 'tools', 'sources', 'tool_types', 'brands', 'prices_ranked']

# these need to have space at the end of each part to create full command
base_command = 'dotnet ef dbcontext scaffold '
db_connection_string = f'"Server=***REMOVED***;Port=5432;Database={database_name};User Id=***REMOVED***;Password=***REMOVED***;" '
nuget_ef_core_provider = 'Npgsql.EntityFrameworkCore.PostgreSQL '
models_output_dir = f'--output-dir {models_dir} '
force_update_flag = '--force '

# exclude because we want all tables to scaffold
# tables = ' '.join(['--table ' + table for table in tables_to_scaffold])
context_name = f'--context {context_name} '
context_output_folder = f'--context-dir {context_dir} '
no_build_flag = '--no-build '

# keep tables last because there is no empty space after the last table
full_command = ( base_command +
                db_connection_string +
                nuget_ef_core_provider +
                context_output_folder +
                models_output_dir +
                context_name +
                force_update_flag +
                no_build_flag
)

print(f'running command: {full_command}')
os.system(full_command)