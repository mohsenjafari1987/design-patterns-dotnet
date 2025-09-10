# .NET Design Patterns Collection

[![Build Status](https://github.com/mohsenjafari1987/design-patterns-dotnet/workflows/CI/badge.svg)](https://github.com/mohsenjafari1987/design-patterns-dotnet/actions)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET 8](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)

A comprehensive collection of software design patterns implemented in .NET 8, featuring real-world examples, unit tests, and practical demonstrations.

## 📋 Project Overview

Design patterns are reusable solutions to commonly occurring problems in software design and development. They represent best practices evolved over time by experienced software developers and serve as templates for writing maintainable, flexible, and robust code.

This repository provides:

- **Clean, well-documented implementations** of classic and modern design patterns
- **Real-world usage scenarios** through practical console applications  
- **Comprehensive test coverage** using xUnit and FluentAssertions
- **Performance benchmarks** for patterns where applicable
- **Modular architecture** with each pattern as an isolated, reusable component

### 🏗️ Architecture Principles

Each design pattern in this repository follows a consistent structure:

- **📚 Core Library**: Pure class library containing the pattern implementation
- **🚀 Sample Application**: Console app demonstrating real-world usage scenarios
- **🧪 Test Project**: Comprehensive unit tests with behavior verification
- **⚡ Benchmarks**: Performance analysis (where relevant)

## 📁 Repository Structure

```
dotnet-design-patterns/
├─ src/
│  ├─ Patterns.Common/              # Shared utilities and common interfaces
│  ├─ Railway/                      # Railway Pattern (Functional)
│  │  ├─ Railway/                   # Core library implementation
│  │  ├─ Railway.Sample/            # Usage examples and demos
│  │  └─ Railway.Tests/             # Unit tests
│  ├─ Strategy/                     # Strategy Pattern (Behavioral)
│  │  ├─ Strategy/                  # Core library implementation
│  │  ├─ Strategy.Sample/           # Usage examples and demos
│  │  └─ Strategy.Tests/            # Unit tests
│  ├─ Factory/                      # Factory Pattern (Creational)
│  │  ├─ Factory/                   # Core library implementation
│  │  ├─ Factory.Sample/            # Usage examples and demos
│  │  └─ Factory.Tests/             # Unit tests
│  ├─ Observer/                     # Observer Pattern (Behavioral)
│  │  ├─ Observer/                  # Core library implementation
│  │  ├─ Observer.Sample/           # Usage examples and demos
│  │  └─ Observer.Tests/            # Unit tests
│  └─ ... (more patterns)
├─ benchmarks/                      # Performance benchmarks
│  ├─ Strategy.Benchmarks/
│  └─ Railway.Benchmarks/
├─ docs/                           # Additional documentation
├─ .editorconfig                   # Code style configuration
├─ .gitignore                      # Git ignore rules
├─ design-patterns-dotnet.sln      # Main solution file
├─ Directory.Build.props           # MSBuild properties
└─ README.md                       # This file
```

### 📂 Folder Descriptions

- **`src/`**: Contains all source code organized by design pattern
- **`benchmarks/`**: Performance analysis and comparison tools
- **`docs/`**: Extended documentation, guides, and architectural decisions
- **Root files**: Solution configuration, build settings, and project metadata

## 🎯 Implemented Patterns

| Pattern | Type | Description | Status |
|---------|------|-------------|--------|
| **Railway** | Functional | Chain operations with error handling, avoiding nested try-catch blocks | 🚧 In Progress |
| **Policy** | Behavioral | Define and apply rules for resilience, retry, and fault handling | 📋 Planned |
| **Decorator** | Structural | Attach additional behavior to objects dynamically without altering structure | 📋 Planned |

> 🔮 **Coming Soon**: More modern patterns including functional programming concepts, reactive patterns, and cloud-native design patterns are being planned for future releases.

### Legend
- ✅ **Done**: Fully implemented with tests and samples
- 🚧 **In Progress**: Currently being developed
- 📋 **Planned**: Scheduled for future implementation

## 🚀 Quick Start

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- Your favorite IDE ([Visual Studio](https://visualstudio.microsoft.com/), [VS Code](https://code.visualstudio.com/), [JetBrains Rider](https://www.jetbrains.com/rider/))

### Running a Pattern Sample

To explore a specific design pattern implementation:

```bash
# Clone the repository
git clone https://github.com/mohsenjafari1987/design-patterns-dotnet.git
cd design-patterns-dotnet

# Run a specific pattern sample (e.g., Railway pattern)
dotnet run --project src/Railway/Railway.Sample

# Run Strategy pattern sample
dotnet run --project src/Strategy/Strategy.Sample
```

### Running Tests

Execute tests for a specific pattern:

```bash
# Test a specific pattern
dotnet test src/Railway/Railway.Tests

# Run all tests with detailed output
dotnet test --logger "console;verbosity=detailed"

# Run tests with coverage (requires coverlet)
dotnet test --collect:"XPlat Code Coverage"
```

### Building the Solution

```bash
# Restore dependencies
dotnet restore

# Build entire solution
dotnet build

# Build in Release mode
dotnet build --configuration Release
```

## 📐 Conventions & Naming Rules

This repository follows strict naming conventions to ensure consistency and discoverability:

### 📂 Pattern Structure
Each pattern follows this exact folder structure:
```
PatternName/
├─ PatternName/              # Core library (e.g., Railway, Strategy)
├─ PatternName.Sample/       # Console application (e.g., Railway.Sample)
└─ PatternName.Tests/        # Unit tests (e.g., Railway.Tests)
```

### 🏷️ Naming Conventions

- **Pattern folders**: Use PascalCase pattern names (e.g., `Railway`, `Strategy`, `FactoryMethod`)
- **Core library**: Same as pattern name
- **Sample projects**: `{PatternName}.Sample`
- **Test projects**: `{PatternName}.Tests`
- **Benchmark projects**: `{PatternName}.Benchmarks` (in `benchmarks/` folder)

### 📚 Shared Components

- **`Patterns.Common`**: Contains shared utilities, base classes, and common interfaces used across multiple patterns
- **Common naming**: All classes, interfaces, and methods follow standard .NET naming conventions

## 🛠️ Technology Stack

| Technology | Purpose | Version |
|------------|---------|---------|
| **.NET** | Runtime and framework | 8.0 |
| **C#** | Programming language | 12.0 |
| **xUnit** | Unit testing framework | Latest |
| **FluentAssertions** | Assertion library for readable tests | Latest |
| **BenchmarkDotNet** | Performance benchmarking | Latest |
| **GitHub Actions** | Continuous Integration | - |

### 📦 NuGet Packages

Core dependencies used across the solution:
- `Microsoft.Extensions.DependencyInjection` - For dependency injection patterns
- `Microsoft.Extensions.Logging` - For logging in samples
- `Newtonsoft.Json` - For serialization examples

## ➕ Adding a New Pattern

Follow these steps to contribute a new design pattern:

### 1. Create Pattern Structure
```bash
# Create pattern folder structure
mkdir src/NewPattern
mkdir src/NewPattern/NewPattern
mkdir src/NewPattern/NewPattern.Sample
mkdir src/NewPattern/NewPattern.Tests
```

### 2. Create Projects
```bash
# Core library
dotnet new classlib -n NewPattern -o src/NewPattern/NewPattern

# Sample console app
dotnet new console -n NewPattern.Sample -o src/NewPattern/NewPattern.Sample

# Test project
dotnet new xunit -n NewPattern.Tests -o src/NewPattern/NewPattern.Tests
```

### 3. Add Project References
```bash
# Sample references core library
dotnet add src/NewPattern/NewPattern.Sample reference src/NewPattern/NewPattern

# Tests reference core library
dotnet add src/NewPattern/NewPattern.Tests reference src/NewPattern/NewPattern

# Add common references
dotnet add src/NewPattern/NewPattern reference src/Patterns.Common
dotnet add src/NewPattern/NewPattern.Tests package FluentAssertions
```

### 4. Update Solution
```bash
# Add projects to solution
dotnet sln add src/NewPattern/NewPattern
dotnet sln add src/NewPattern/NewPattern.Sample
dotnet sln add src/NewPattern/NewPattern.Tests
```

### 5. Implement and Document
- Implement the pattern in the core library
- Create meaningful examples in the sample project
- Write comprehensive tests
- Update the patterns table in this README
- Add documentation to the `docs/` folder

## 🔨 Build & Test Instructions

### Local Development

```bash
# Full development cycle
dotnet restore                    # Restore packages
dotnet build                      # Build all projects
dotnet test                       # Run all tests
dotnet pack                       # Create NuGet packages (if applicable)
```

### Continuous Integration

The project uses GitHub Actions for automated building and testing:

- **Build verification**: Runs on every push and pull request
- **Multi-platform testing**: Tests on Windows, Linux, and macOS
- **Code quality checks**: Includes linting and formatting verification
- **Test coverage**: Generates and reports code coverage metrics

### Code Quality

Maintain high code quality with:
- **EditorConfig**: Consistent code formatting
- **Analyzer rules**: Static code analysis
- **Unit tests**: Minimum 80% code coverage
- **Documentation**: XML comments for public APIs

## 📖 Documentation

For detailed information about specific patterns:

- Each pattern includes inline XML documentation
- Sample projects demonstrate real-world usage scenarios
- Test projects serve as living documentation of expected behavior
- Additional guides available in the `docs/` folder

## 🤝 Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/new-pattern`)
3. Follow the established conventions and structure
4. Add comprehensive tests and documentation
5. Submit a pull request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 🙏 Acknowledgments

- **Gang of Four**: For the foundational "Design Patterns" book
- **.NET Community**: For continuous inspiration and feedback
- **Contributors**: Everyone who helps improve this repository

---

⭐ **Star this repository** if you find it helpful for learning design patterns in .NET!