﻿#region	License

//------------------------------------------------------------------------------------------------
// <License>
//     <Copyright> 2017 © Top Nguyen → AspNetCore → Monkey </Copyright>
//     <Url> http://topnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> Monkey </Project>
//     <File>
//         <Name> Enums.cs </Name>
//         <Created> 17/04/2017 01:50:48 AM </Created>
//         <Key> 88d69d6a-68b9-475a-8ff1-73aa35ff0711 </Key>
//     </File>
//     <Summary>
//         Enums.cs
//     </Summary>
// <License>
//------------------------------------------------------------------------------------------------

#endregion License

using System.ComponentModel.DataAnnotations;

namespace Monkey.Core
{
    public class Enums
    {
        public enum UserRole
        {
            [Display(Name = "Developer")]
            Developer = 1000,

            [Display(Name = "Admin")]
            Admin = 2000,

            [Display(Name = "Manager")]
            Manager = 3000
        }
    }
}