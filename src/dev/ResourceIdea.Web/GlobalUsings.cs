global using System;
global using System.Collections.Generic;
global using System.ComponentModel.DataAnnotations;
global using System.Net;

global using Microsoft.AspNetCore.Authentication;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Identity;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Mvc.RazorPages;
global using Microsoft.EntityFrameworkCore;

global using ResourceIdea.Common.Constants;
global using ResourceIdea.Common.Exceptions;
global using ResourceIdea.Common.Extensions;
global using ResourceIdea.Middleware;
global using ResourceIdea.Models;
global using ResourceIdea.Web.Constants;
global using ResourceIdea.Web.Core.Handlers.Clients;
global using ResourceIdea.Web.Core.Handlers.Engagements;
global using ResourceIdea.Web.Core.Handlers.Tasks;
global using ResourceIdea.Web.Core.ViewModels;
global using ResourceIdea.Web.Core.ViewModels.Clients;
global using ResourceIdea.Web.Exceptions;
global using ResourceIdea.Web.Infrastructure.Auth;
global using ResourceIdea.Web.Infrastructure.Environment;
