using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Haro.AdminPanel.Common;
using Haro.AdminPanel.Models.CustomModels;
using Haro.AdminPanel.Models.Entities;
using Haro.AdminPanel.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace Haro.AdminPanel.Business.Managers
{
    public class ExportManager
    {
        private TableManager _tableManager;
        private ModuleManager _moduleManager;
        private ColumnManager _columnManager;
        private LanguageManager _languageManager;
        private SiteInformationManager _siteInformationManager;
        private UserManager _userManager;
        private UserModuleManager _userModuleManager;

        public ExportManager(TableManager tableManager, ModuleManager moduleManager, ColumnManager columnManager, LanguageManager languageManager, SiteInformationManager siteInformationManager, UserManager userManager, UserModuleManager userModuleManager)
        {
            _tableManager = tableManager;
            _moduleManager = moduleManager;
            _columnManager = columnManager;
            _languageManager = languageManager;
            _siteInformationManager = siteInformationManager;
            _userManager = userManager;
            _userModuleManager = userModuleManager;
        }

        public void Export()
        {
            var saveDir = "/Models/Entities/";

            var tables = _tableManager.BaseQuery().Include(x => x.Columns).ToList();
            foreach (var item in tables)
            {
                var model = new CreateClassModel()
                {
                    Dependency = "Entry",
                    Namespace = $"{App.Common.SiteInformation.WebProjectName}.Models.Entities",
                    Name = item.Name,
                    Imports = new List<string>() {"Haro.AdminPanel.Models.Entities"}
                };
                foreach (var column in item.Columns)
                {
                    switch (column.ColumnType)
                    {
                        case ColumnType.Text:
                        case ColumnType.Password:
                        case ColumnType.TextArea:
                        case ColumnType.Editor:
                        case ColumnType.Image:
                        case ColumnType.Number:
                        case ColumnType.Bool:
                        case ColumnType.Slug:
                            model.Properties.Add(
                                new KeyValuePair<string, string>(ColumnTypeToStr(column.ColumnType), column.Name));
                            break;
                        case ColumnType.SelectList:
                            model.Imports.Add("System.Collections.Generic");
                            model.Properties.Add(new KeyValuePair<string, string>(column.TargetTable, column.Name));
                            model.Properties.Add(new KeyValuePair<string, string>("long", column.Name + "Id"));
                            break;
                        case ColumnType.MultipleSelectList:
                            model.Imports.Add("System.Collections.Generic");
                            model.Properties.Add(new KeyValuePair<string, string>(
                                $"List<{column.Table.Name}{column.Name}Image>", column.Name));
                            CreateCsFile(saveDir, new CreateClassModel()
                            {
                                Name = item.Name + column.TargetTable,
                                Namespace = $"{App.Common.SiteInformation.WebProjectName}.Models.Entities",
                                Properties = new List<KeyValuePair<string, string>>()
                                {
                                    new KeyValuePair<string, string>(item.Name, item.Name),
                                    new KeyValuePair<string, string>("long", item.Name + "Id"),
                                    new KeyValuePair<string, string>(column.TargetTable, column.TargetTable),
                                    new KeyValuePair<string, string>("long", column.TargetTable + "Id"),
                                }
                            });
                            break;
                        case ColumnType.MultipleImage:
                            model.Imports.Add("System.Collections.Generic");
                            model.Properties.Add(new KeyValuePair<string, string>(
                                $"List<{item.Name}Image>", item.Name + "Image"));
                            CreateCsFile(saveDir, new CreateClassModel()
                            {
                                Name = item.Name + "Image",
                                Namespace = $"{App.Common.SiteInformation.WebProjectName}.Models.Entities",
                                Properties = new List<KeyValuePair<string, string>>()
                                {
                                    new KeyValuePair<string, string>(item.Name, item.Name),
                                    new KeyValuePair<string, string>("long", item.Name + "Id"),
                                    new KeyValuePair<string, string>("string", "Image"),
                                }
                            });
                            break;
                        case ColumnType.Hidden:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                CreateCsFile(saveDir, model);
            }
        }

        private string ColumnTypeToStr(ColumnType type)
        {
            switch (type)
            {
                case ColumnType.Text:
                case ColumnType.TextArea:
                case ColumnType.Editor:
                case ColumnType.Image:
                case ColumnType.Password:
                case ColumnType.Slug:
                    return "string";
                case ColumnType.Number:
                    return "int";
                case ColumnType.Bool:
                    return "bool";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }


        private string CreateCsFile(string path, CreateClassModel model)
        {
            path = Directory.GetCurrentDirectory() + path;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var fileName = path + $"{model.Name}.cs";
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            File.WriteAllText(fileName, CreateClass(model));
            return fileName;
        }

        private CreateClassModel EntryModel => new CreateClassModel()
        {
            Imports = new List<string>() {"Haro.AdminPanel.Models.Entities"},
            Namespace = $"{App.Common.SiteInformation.WebProjectName}.Models.Entities",
            Name = "Entry",
            Properties = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("long", "Id"),
                new KeyValuePair<string, string>("Language", "Language"),
                new KeyValuePair<string, string>("long", "LanguageId"),
            }
        };


        private string CreateClass(CreateClassModel model)
        {
            var str = "";

            foreach (var item in model.Imports)
            {
                str += $"using {item};\n";
            }

            if (model.Imports.Any())
            {
                str += "\n";
            }


            str += $"namespace {model.Namespace}\n";
            str += "{\n";
            str += $"\tpublic class {model.Name}";
            if (!string.IsNullOrEmpty(model.Dependency))
            {
                str += $" : {model.Dependency}";
            }

            str += "\n";
            str += "\t{\n";

            foreach (var item in model.Properties)
            {
                str += CreateProperty(item.Key, item.Value);
            }

            str += "\t}\n";
            str += "}";

            return str;
        }

        private string CreateProperty(string type, string name)
        {
            return $"\t\tpublic {type} {name} {{ get; set; }}\n";
        }
        public class CreateClassModel
        {
            public List<string> Imports { get; set; } = new List<string>();
            public string Namespace { get; set; }
            public string Name { get; set; }
            public List<KeyValuePair<string, string>> Properties { get; set; } = new List<KeyValuePair<string, string>>();
            public string Dependency { get; set; }
        }
        public ImportExportDataModel ExportSchema()
        {
            return new ImportExportDataModel
            {
                Languages = _languageManager.List(),
                Modules = _moduleManager.List().Select(x=>{ x.Tables = new List<Table>(); x.Users = new List<UserModule>(); return x; }).ToList(),
                Tables = _tableManager.List().Select(x => { x.Columns = new List<Column>(); x.Module = null; return x; }).ToList(),
                Columns = _columnManager.List().Select(x => { x.Table = null; return x; }).ToList(),
                SiteInformations = _siteInformationManager.List(),
                Users = _userManager.List().Select(x => { x.Modules = new List<UserModule>(); return x; }).ToList(),    
                UserModules = _userModuleManager.List().Select(x => { x.Module = null; x.User = null; return x; }).ToList(),
            };
        }
    }
}