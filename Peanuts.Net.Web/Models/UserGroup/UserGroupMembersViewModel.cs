﻿using System.Collections.Generic;
using Com.QueoFlow.Peanuts.Net.Core.Domain.Users;
using Com.QueoFlow.Peanuts.Net.Core.Infrastructure.Checks;

namespace Com.QueoFlow.Peanuts.Net.Web.Models.UserGroup {
    /// <summary>
    ///     ViewModel für die Anzeige einer Gruppen-Mitgliedschaft.
    /// </summary>
    public class UserGroupMembersViewModel {
        public UserGroupMembersViewModel(Core.Domain.Users.UserGroup userGroup, UserGroupMembership currentUsersMembershipInGroup, IList<UserGroupMembership> currentMembers, IList<UserGroupMembership> requestingMembers, IList<UserGroupMembership> invitedMembers, IList<UserGroupMembership> formerMembers, UserGroupMembershipOptions userGroupMembershipOptions) {
            Require.NotNull(userGroup, "userGroup");
            Require.NotNull(currentMembers, "currentMembers");
            Require.NotNull(formerMembers, "formerMembers");
            Require.NotNull(userGroupMembershipOptions, "userGroupMembershipOptions");

            UserGroup = userGroup;
            CurrentUsersMembershipInGroup = currentUsersMembershipInGroup;
            CurrentUserGroupMembers = currentMembers;
            RequestingMembers = requestingMembers;
            InvitedMembers = invitedMembers;
            FormerUserGroupMembers = formerMembers;
            UserGroupMembershipOptions = userGroupMembershipOptions;
        }

        /// <summary>
        /// Ruft die angezeigte Nutzergruppe ab.
        /// </summary>
        public Core.Domain.Users.UserGroup UserGroup {
            get;
        }

        public UserGroupMembership CurrentUsersMembershipInGroup { get; }

        /// <summary>
        ///     Ruft die Mitgliedschaften der Gruppe ab.
        /// </summary>
        public IList<UserGroupMembership> CurrentUserGroupMembers {
            get;
        }

        /// <summary>
        /// Ruft die Liste der Nutzer ab, die eingeladenen wurden.
        /// </summary>
        public IList<UserGroupMembership> InvitedMembers {
            get;
        }

        /// <summary>
        /// Ruft die Liste der Nutzer ab, die eine Mitgliedschaft angefragt haben.
        /// </summary>
        public IList<UserGroupMembership> RequestingMembers {
            get;
        }

        /// <summary>
        ///     Ruft die ehemaligen Mitgliedschaften der Gruppe ab.
        /// </summary>
        public IList<UserGroupMembership> FormerUserGroupMembers {
            get;
        }

        /// <summary>
        ///     Ruft die Optionen der Seite ab.
        /// </summary>
        public UserGroupMembershipOptions UserGroupMembershipOptions {
            get;
        }
    }
}