using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace FingergunsApi.App.Helpers
 {
     /// <summary>
     /// Classes that implement this interface can be used to hash and check passwords.
     /// </summary>
     public interface IPasswordHelper
     {
         /// <summary>
         /// This method hashes the provided <paramref name="password" /> with a randomly generated
         /// salt value according to the given <paramref name="policy" />. A tuple containing the
         /// generated hash and salt is returned.
         /// </summary>
         /// <param name="password">The password to be hashed.</param>
         /// <param name="policy">A policy containing options for hashing.</param>
         /// <returns>A tuple containing (hash, salt).</returns>
         public (string hash, string salt) HashPassword(string password, HashingPolicy policy);
 
         /// <summary>
         /// This method hashes the given <paramref name="password" /> with the default hashing
         /// policy.
         /// </summary>
         /// <param name="password">The password to hash.</param>
         /// <returns>A tuple containing (hash, salt).</returns>
         public (string hash, string salt) HashPassword(string password);
 
         /// <summary>
         /// Verifies that the given <paramref="password" /> matches the given <paramref="hash" />
         /// and <paramref name="salt" /> according to the given <paramref name="policy" />.
         /// </summary>
         /// <param name="password">The password to check.</param>
         /// <param name="hash">The hash of the password (usually retrieved from DB).</param>
         /// <param name="salt">The salt of the password (usually retrieved from DB).</param>
         /// <param name="policy">A policy containing options for hashing.</param>
         /// <returns>
         /// True if the password matches the given hash and salt according to
         /// the given policy, false otherwise.
         /// </returns>
         public bool IsPasswordValid(string password, string hash, string salt, HashingPolicy policy);
 
         /// <summary>
         /// Verifies that the given <paramref="password" /> matches the given <paramref="hash" />
         /// and <paramref name="salt" /> according to the default hashing policy.
         /// </summary>
         /// <param name="password">The password to check.</param>
         /// <param name="hash">The hash of the password (usually retrieved from DB).</param>
         /// <param name="salt">The salt of the password (usually retrieved from DB).</param>
         /// <returns>
         /// True if the password matches the given hash and salt according to
         /// the default policy, false otherwise.
         /// </returns>
         public bool IsPasswordValid(string password, string hash, string salt);
     }
 
     public class PasswordHelper : IPasswordHelper
     {
         public (string hash, string salt) HashPassword(string password, HashingPolicy policy)
         {
             var salt = new byte[policy.SaltLength];
             policy.Rng.GetBytes(salt);
             var saltString = Convert.ToBase64String(salt);
 
             var hash = Hash(password, saltString, policy);
 
             return (hash, saltString);
         }
 
         public (string hash, string salt) HashPassword(string password)
         {
             return HashPassword(password, new HashingPolicy());
         }
 
         public bool IsPasswordValid(string password, string hash, string salt, HashingPolicy policy)
         {
             var hashedPass = Hash(password, salt, policy);
             return hashedPass.Equals(hash);
         }
 
         public bool IsPasswordValid(string password, string hash, string salt)
         {
             return IsPasswordValid(password, hash, salt, new HashingPolicy());
         }
 
         private string Hash(string password, string salt, HashingPolicy policy)
         {
             var saltBytes = Convert.FromBase64String(salt);
             return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                 password: password,
                 salt: saltBytes,
                 prf: policy.Prf,
                 iterationCount: policy.IterCount,
                 numBytesRequested: policy.NumBytesRequested
             ));
         }
     }
 
     /// <summary>
     /// This class contains options that control how hashing is done.
     /// </summary>
     public class HashingPolicy
     {
         public int IterCount { get; set; } = 10000;
         public int SaltLength { get; set; } = 128 / 8;
         public int NumBytesRequested { get; set; } = 256 / 8;
         public KeyDerivationPrf Prf { get; set; } = KeyDerivationPrf.HMACSHA256;
         public RandomNumberGenerator Rng { get; set; } = RandomNumberGenerator.Create();
     }
 }