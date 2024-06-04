using Microsoft.AspNetCore.Authorization;
using System;

namespace rethus_backend.Models
{
    public static class Policies
    {
        public const string Admin = "Admin";
        public const string User = "User";
        public const string SuperAdmin = "SuperAdmin";
        public const string FuncionarioEtapa1 = "FuncionarioEtapa1";
        public const string FuncionarioEtapa2 = "FuncionarioEtapa2";
        public const string FuncionarioEtapa3 = "FuncionarioEtapa3";
        public const string Inventory = "Inventory";

        public static AuthorizationPolicy AdminPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(Admin)
                .Build();
        }

        public static AuthorizationPolicy UserPolicy()
        {
            return new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .RequireRole(User)
                .Build();
        }
    }
}
