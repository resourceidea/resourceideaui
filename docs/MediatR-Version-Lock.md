# MediatR Version Lock

## Current Version: 11.1.0

**⚠️ WARNING: DO NOT UPGRADE MediatR beyond version 11.x**

## Why is MediatR locked to version 11.1.0?

Starting with **MediatR version 13.0.0**, Lucky Penny Software introduced licensing requirements for commercial use. This creates the following issues:

1. **Runtime Licensing Warnings**: Applications display licensing warnings during startup
2. **Commercial License Required**: Production deployments require purchasing a commercial license
3. **Legal Compliance**: Using unlicensed commercial versions may violate licensing terms

## What has been implemented?

### 1. Dependabot Configuration
- **File**: `.github/dependabot.yml`
- **Purpose**: Prevents automated dependency update PRs for MediatR packages
- **Packages Ignored**: 
  - `MediatR`
  - `MediatR.*` (all MediatR-related packages)
  - `MediatR.Extensions.Microsoft.DependencyInjection`

### 2. Central Version Management
- **File**: `Directory.Build.props`
- **Purpose**: Centrally manages MediatR versions across all projects
- **Properties**:
  - `MediatRVersion`: 11.1.0
  - `MediatRExtensionsVersion`: 11.1.0

### 3. Project References
All projects now use MSBuild properties instead of hardcoded versions:
```xml
<PackageReference Include="MediatR" Version="$(MediatRVersion)" />
<PackageReference Include="MediatR.Extensions.Microsoft.DependencyInjection" Version="$(MediatRExtensionsVersion)" />
```

## If you need to upgrade MediatR

### Option 1: Commercial License (Recommended for Production)
1. Purchase a commercial license from [Lucky Penny Software](https://luckypennysoftware.com)
2. Update `Directory.Build.props` with the desired version
3. Add license configuration to your application startup

### Option 2: Alternative Libraries
Consider migrating to alternative CQRS/Mediator libraries:
- **Wolverine**: Modern .NET mediator with high performance
- **MassTransit Mediator**: Part of the MassTransit ecosystem
- **Custom Implementation**: Roll your own mediator pattern

### Option 3: Stay on 11.1.0 (Current Choice)
- ✅ No licensing costs
- ✅ No licensing warnings
- ✅ Stable and well-tested
- ✅ Supports all current application features
- ❌ No future updates or bug fixes
- ❌ May become incompatible with future .NET versions

## Version History

| Version | Status | Notes |
|---------|--------|--------|
| 11.1.0 | ✅ Current | Last open-source version |
| 12.x | ⚠️ Transitional | Some licensing introduced |
| 13.0.0+ | ❌ Commercial | Requires license for production |

## Related Links

- [MediatR GitHub Issue #868](https://github.com/jbogard/MediatR/issues/868) - Licensing discussion
- [Lucky Penny Software](https://luckypennysoftware.com) - Commercial licensing
- [Dependabot Configuration](https://docs.github.com/github/administering-a-repository/configuration-options-for-dependency-updates) - GitHub documentation

---

**Last Updated**: July 17, 2025  
**Maintainer**: Development Team
