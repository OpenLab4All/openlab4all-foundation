# OpenLab4All Foundation

**OpenLab4All Foundation** is a collection of reusable libraries built on **.NET Standard 2.0**, designed to provide core functionality for any type of .NET application.  
Each library is distributed as an **independent NuGet package**, so you can pick only what you need.

> Licensed under the **Apache 2.0 License**, allowing use in both open-source and proprietary software.

---

## 📦 Packages

- **OpenLab4All.Foundation.Core**  
  Core contracts and minimal utilities.

- **OpenLab4All.Foundation.Diagnostics**  
  Lightweight logging, tracing, and timing helpers.

- **OpenLab4All.Foundation.Extensions**  
  Useful extension methods for collections, enums, LINQ, DateTime, etc.

- **OpenLab4All.Foundation.Text**  
  String utilities: normalization, comparators, slugify, parsers.

- **OpenLab4All.Foundation.Text.Formatting**  
  Cultural and formatting helpers for dates, numbers, and strings.

- **OpenLab4All.Foundation.IO**  
  File and stream helpers, temporary directories, compression.

- **OpenLab4All.Foundation.Data.Abstractions**  
  Interfaces and contracts for database connections, query execution, and unit of work.

- **OpenLab4All.Foundation.Data.Mapping**  
  Safe data mappers from `IDataRecord` or `DataRow` to POCOs.

- **OpenLab4All.Foundation.Data.Pagination**  
  Paging models and query helpers.

- **OpenLab4All.Foundation.Data.SqlServer**  
  SQL Server implementations of the data abstractions.

- **OpenLab4All.Foundation.Http**  
  HTTP client utilities and abstractions.

- **OpenLab4All.Foundation.Http.Ssrs**  
  Helpers for building and executing SSRS report requests.

---

## 🚀 Getting Started

Install the package you need via NuGet:

```bash
dotnet add package OpenLab4All.Foundation.Text
```

Example usage:

```csharp
using OpenLab4All.Foundation.Text;

string slug = Slugifier.ToSlug("Hello World!");
// slug = "hello-world"
```

---

## 🏗️ Repository Structure

```
/src
  /OpenLab4All.Foundation.Core
  /OpenLab4All.Foundation.Text
  /OpenLab4All.Foundation.Data.SqlServer
/tests
  /OpenLab4All.Foundation.Core.Tests
  /OpenLab4All.Foundation.Text.Tests
```

---

## 📖 License

This project is licensed under the [Apache License 2.0](LICENSE).
