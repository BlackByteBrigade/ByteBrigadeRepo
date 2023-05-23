using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
   public class EnermyAttacking : Enemy
    {
        public float AttackCooldown;

        public bool AttackCoolDowned => LastAttacked == default || (DateTime.Now - LastAttacked).TotalSeconds > AttackCooldown;
        public DateTime LastAttacked { get; set; }

        public void Attack()
        {
            LastAttacked = DateTime.Now;
        }
    }
}
