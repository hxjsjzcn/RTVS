﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Common.Core;

namespace Microsoft.R.Components.Controller {
    public class AsyncCommandController : ICommandTarget {
        private Dictionary<Key, IAsyncCommand> CommandMap { get; } = new Dictionary<Key, IAsyncCommand>();

        public CommandResult Invoke(Guid group, int id, object inputArg, ref object outputArg) {
            CommandResult result = CommandResult.NotSupported;
            var cmd = Find(group, id);

            // Pre-process the command
            if (cmd != null) {
                cmd.InvokeAsync().DoNotWait();
                result = CommandResult.Executed;
            }

            return result;
        }

        public void PostProcessInvoke(CommandResult result, Guid group, int id, object inputArg, ref object outputArg) {
        }

        public CommandStatus Status(Guid group, int id) {
            IAsyncCommand cmd = Find(group, id);
            if (cmd != null) {
                return cmd.Status;
            }

            return CommandStatus.NotSupported;
        }

        /// <summary>
        /// Adds command to the controller command table
        /// </summary>
        /// <param name="command">Command object</param>
        public void AddCommand(Guid group, int id, IAsyncCommand command) {
            var key = new Key(group, id);
            if (!CommandMap.ContainsKey(key)) {
                CommandMap.Add(key, command);
            }
        }

        public IAsyncCommand Find(Guid group, int id) {
            IAsyncCommand cmd = null;
            CommandMap.TryGetValue(new Key(group, id), out cmd);
            return cmd;
        }

        private struct Key : IEquatable<Key> {
            public Guid Group { get; }
            public int Id { get; }

            public Key(Guid group, int id) {
                Group = group;
                Id = id;
            }

            public override bool Equals(object obj) => obj is Key && Equals((Key)obj);

            public bool Equals(Key other) => Group.Equals(other.Group) && Id == other.Id;

            public override int GetHashCode() {
                unchecked {
                    return (Group.GetHashCode() * 397) ^ Id;
                }
            }
        }
    }
}
