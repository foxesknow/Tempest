﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Threading;

namespace Tempest.Expressions
{
    public static partial class ExpressionEx
    {
        private static readonly MethodInfo s_MonitorEnter = GetMethod(typeof(Monitor), "Enter", BindingFlags.Public | BindingFlags.Static, typeof(object), typeof(bool).MakeByRefType());
        private static readonly MethodInfo s_MonitorExit = GetMethod(typeof(Monitor), "Exit", BindingFlags.Public | BindingFlags.Static, typeof(object));

        public static Expression Lock(Expression lockObject, Expression body)
        {
            if(lockObject == null) throw new ArgumentNullException(nameof(lockObject)); 
            if(body == null) throw new ArgumentNullException(nameof(body)); 

            if(lockObject.Type.IsValueType) throw new ArgumentException("cannot lock on a value type", nameof(lockObject));

            var lockVariable = Variable<object>("lock");
            var lockInit = lockObject.Type.IsValueType switch
            {
                true => Expression.Assign(lockVariable, Expression.Convert(lockObject, typeof(object))),
                _    => Expression.Assign(lockVariable, lockObject)
            };

            var lockTakenVariable = Variable<bool>("lockTaken");
            var lockTakenInit = Expression.Assign(lockTakenVariable, Constants.Bool.False);

            var tryFinally = Expression.TryFinally
            (
                Expression.Block
                (
                    Expression.Call(s_MonitorEnter, lockVariable, lockTakenVariable),
                    body
                ),
                Expression.Block
                (
                    Expression.IfThen
                    (
                        lockTakenVariable,
                        Expression.Call(s_MonitorExit, lockVariable)
                    )
                )
            );

            var block = Expression.Block
            (
                new []{lockVariable, lockTakenVariable},
                lockInit,
                lockTakenInit,
                tryFinally
            );

            return block;
        }
    }
}