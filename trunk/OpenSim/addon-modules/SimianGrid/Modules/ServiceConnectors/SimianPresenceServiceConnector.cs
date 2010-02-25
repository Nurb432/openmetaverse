﻿/* 
 * Copyright (c) Intel Corporation
 * All rights reserved.
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions
 * are met:
 *
 * -- Redistributions of source code must retain the above copyright
 *    notice, this list of conditions and the following disclaimer.
 * -- Redistributions in binary form must reproduce the above copyright
 *    notice, this list of conditions and the following disclaimer in the
 *    documentation and/or other materials provided with the distribution.
 * -- Neither the name of the Intel Corporation nor the names of its
 *    contributors may be used to endorse or promote products derived from
 *    this software without specific prior written permission.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
 * ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
 * PARTICULAR PURPOSE ARE DISCLAIMED.  IN NO EVENT SHALL THE INTEL OR ITS
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,
 * PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
 * PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
 * LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Reflection;
using Mono.Addins;
using log4net;
using Nini.Config;
using OpenSim.Framework;
using OpenSim.Framework.Servers.HttpServer;
using OpenSim.Region.Framework.Interfaces;
using OpenSim.Region.Framework.Scenes;
using OpenSim.Services.Interfaces;
using OpenSim.Server.Base;
using OpenMetaverse;
using OpenMetaverse.StructuredData;

using PresenceInfo = OpenSim.Services.Interfaces.PresenceInfo;

namespace SimianGrid
{
    [Extension(Path = "/OpenSim/RegionModules", NodeName = "RegionModule")]
    public class SimianPresenceServiceConnector : IPresenceService, ISharedRegionModule
    {
        private static readonly ILog m_log =
                LogManager.GetLogger(
                MethodBase.GetCurrentMethod().DeclaringType);

        private string m_serverUrl = String.Empty;

        #region ISharedRegionModule

        public Type ReplaceableInterface { get { return null; } }
        public void RegionLoaded(Scene scene) { }
        public void PostInitialise() { }
        public void Close() { }

        public SimianPresenceServiceConnector() { }
        public string Name { get { return "SimianPresenceServiceConnector"; } }
        public void AddRegion(Scene scene)
        {
            scene.RegisterModuleInterface<IPresenceService>(this);

            scene.EventManager.OnMakeRootAgent += MakeRootAgentHandler;
            scene.EventManager.OnNewClient += NewClientHandler;

            LogoutRegionAgents(scene.RegionInfo.RegionID);
        }
        public void RemoveRegion(Scene scene)
        {
            scene.UnregisterModuleInterface<IPresenceService>(this);

            scene.EventManager.OnMakeRootAgent -= MakeRootAgentHandler;
            scene.EventManager.OnNewClient -= NewClientHandler;

            LogoutRegionAgents(scene.RegionInfo.RegionID);
        }

        #endregion ISharedRegionModule

        public SimianPresenceServiceConnector(IConfigSource source)
        {
            Initialise(source);
        }

        public void Initialise(IConfigSource source)
        {
            IConfig gridConfig = source.Configs["PresenceService"];
            if (gridConfig == null)
            {
                m_log.Error("[PRESENCE CONNECTOR]: PresenceService missing from OpenSim.ini");
                throw new Exception("Presence connector init error");
            }

            string serviceUrl = gridConfig.GetString("PresenceServerURI");
            if (String.IsNullOrEmpty(serviceUrl))
            {
                m_log.Error("[PRESENCE CONNECTOR]: No PresenceServerURI in section PresenceService");
                throw new Exception("Presence connector init error");
            }

            m_serverUrl = serviceUrl;
        }

        #region IPresenceService

        public bool LoginAgent(string userID, UUID sessionID, UUID secureSessionID)
        {
            m_log.ErrorFormat("[PRESENCE CONNECTOR]: Login requested, UserID={0}, SessionID={1}, SecureSessionID={2}",
                userID, sessionID, secureSessionID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "AddSession" },
                { "UserID", userID.ToString() }
            };
            if (sessionID != UUID.Zero)
            {
                requestArgs["SessionID"] = sessionID.ToString();
                requestArgs["SecureSessionID"] = secureSessionID.ToString();
            }

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            bool success = response["Success"].AsBoolean();

            if (!success)
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to login agent " + userID + ": " + response["Message"].AsString());

            return success;
        }

        public bool LogoutAgent(UUID sessionID, Vector3 position, Vector3 lookat)
        {
            m_log.InfoFormat("[PRESENCE CONNECTOR]: Logout requested for agent with sessionID " + sessionID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "RemoveSession" },
                { "SessionID", sessionID.ToString() }
            };

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            bool success = response["Success"].AsBoolean();

            if (!success)
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to logout agent with sessionID " + sessionID + ": " + response["Message"].AsString());

            return success;
        }

        public bool LogoutRegionAgents(UUID regionID)
        {
            m_log.InfoFormat("[PRESENCE CONNECTOR]: Logout requested for all agents in region " + regionID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "RemoveSessions" },
                { "SceneID", regionID.ToString() }
            };

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            bool success = response["Success"].AsBoolean();

            if (!success)
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to logout agents from region " + regionID + ": " + response["Message"].AsString());

            return success;
        }

        public bool ReportAgent(UUID sessionID, UUID regionID, Vector3 position, Vector3 lookAt)
        {
            m_log.DebugFormat("[PRESENCE CONNECTOR]: Updating session data for agent with sessionID " + sessionID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "UpdateSession" },
                { "SessionID", sessionID.ToString() },
                { "SceneID", regionID.ToString() },
                { "ScenePosition", position.ToString() },
                { "SceneLookAt", lookAt.ToString() }
            };

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            bool success = response["Success"].AsBoolean();

            if (!success)
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to update agent session " + sessionID + ": " + response["Message"].AsString());

            return success;
        }

        public PresenceInfo GetAgent(UUID sessionID)
        {
            m_log.DebugFormat("[PRESENCE CONNECTOR]: Requesting session data for agent with sessionID " + sessionID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "GetSession" },
                { "SessionID", sessionID.ToString() }
            };

            OSDMap sessionResponse = WebUtil.PostToService(m_serverUrl, requestArgs);
            if (sessionResponse["Success"].AsBoolean())
            {
                UUID userID = sessionResponse["UserID"].AsUUID();
                m_log.DebugFormat("[PRESENCE CONNECTOR]: Requesting user data for " + userID);

                requestArgs = new NameValueCollection
                {
                    { "RequestMethod", "GetUser" },
                    { "UserID", userID.ToString() }
                };

                OSDMap userResponse = WebUtil.PostToService(m_serverUrl, requestArgs);
                if (userResponse["Success"].AsBoolean())
                    return ResponseToPresenceInfo(sessionResponse, userResponse);
                else
                    m_log.Warn("[PRESENCE CONNECTOR]: Failed to retrieve user data for " + userID + ": " + userResponse["Message"].AsString());
            }
            else
            {
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to retrieve session " + sessionID + ": " + sessionResponse["Message"].AsString());
            }

            return null;
        }

        public PresenceInfo[] GetAgents(string[] userIDs)
        {
            List<PresenceInfo> presences = new List<PresenceInfo>(userIDs.Length);

            for (int i = 0; i < userIDs.Length; i++)
            {
                UUID userID;
                if (UUID.TryParse(userIDs[i], out userID))
                    presences.AddRange(GetSessions(userID));
            }

            return presences.ToArray();
        }

        public bool SetHomeLocation(string userID, UUID regionID, Vector3 position, Vector3 lookAt)
        {
            m_log.DebugFormat("[PRESENCE CONNECTOR]: Setting home location for user  " + userID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "AddUserData" },
                { "UserID", userID.ToString() },
                { "HomeSceneID", regionID.ToString() },
                { "HomePosition", position.ToString() },
                { "HomeLookAt", lookAt.ToString() }
            };

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            bool success = response["Success"].AsBoolean();

            if (!success)
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to set home location for " + userID + ": " + response["Message"].AsString());

            return success;
        }

        #endregion IPresenceService

        #region Presence Detection

        private void MakeRootAgentHandler(ScenePresence sp)
        {
            m_log.DebugFormat("[PRESENCE DETECTOR]: Detected root presence {0} in {1}", sp.UUID, sp.Scene.RegionInfo.RegionName);
            ReportAgent(sp.ControllingClient.SessionId, sp.Scene.RegionInfo.RegionID, sp.AbsolutePosition, sp.Lookat);
        }

        private void NewClientHandler(IClientAPI client)
        {
            client.OnLogout += LogoutHandler;
        }

        private void LogoutHandler(IClientAPI client)
        {
            client.OnLogout -= LogoutHandler;

            ScenePresence sp = null;
            Vector3 position = new Vector3(128f, 128f, 0f);
            Vector3 lookat = Vector3.UnitX;

            if (client.Scene is Scene && ((Scene)client.Scene).TryGetAvatar(client.AgentId, out sp))
            {
                position = sp.AbsolutePosition;
                lookat = sp.Lookat;
            }

            LogoutAgent(client.SessionId, position, lookat);
        }

        #endregion Presence Detection

        #region Helpers

        private OSDMap GetUserData(UUID userID)
        {
            m_log.DebugFormat("[PRESENCE CONNECTOR]: Requesting user data for " + userID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "GetUserData" },
                { "UserID", userID.ToString() }
            };

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            if (response["Success"].AsBoolean())
                return response;
            else
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to retrieve user data for " + userID);

            return null;
        }

        private OSDMap GetSessionData(UUID sessionID)
        {
            m_log.DebugFormat("[PRESENCE CONNECTOR]: Requesting session data for session " + sessionID);

            NameValueCollection requestArgs = new NameValueCollection
            {
                { "RequestMethod", "GetSession" },
                { "SessionID", sessionID.ToString() }
            };

            OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
            if (response["Success"].AsBoolean())
                return response;
            else
                m_log.Warn("[PRESENCE CONNECTOR]: Failed to retrieve session data for session " + sessionID);

            return null;
        }

        private List<PresenceInfo> GetSessions(UUID userID)
        {
            List<PresenceInfo> presences = new List<PresenceInfo>(1);

            OSDMap userResponse = GetUserData(userID);
            if (userResponse != null)
            {
                m_log.DebugFormat("[PRESENCE CONNECTOR]: Requesting sessions for " + userID);

                NameValueCollection requestArgs = new NameValueCollection
                {
                    { "RequestMethod", "GetSessions" },
                    { "UserID", userID.ToString() }
                };

                OSDMap response = WebUtil.PostToService(m_serverUrl, requestArgs);
                if (response["Success"].AsBoolean())
                {
                    OSDArray array = response["Sessions"] as OSDArray;
                    if (array != null)
                    {
                        for (int i = 0; i < array.Count; i++)
                        {
                            PresenceInfo presence = ResponseToPresenceInfo(array[i] as OSDMap, userResponse);
                            if (presence != null)
                                presences.Add(presence);
                        }
                    }
                    else
                    {
                        m_log.Warn("[PRESENCE CONNECTOR]: GetSessions returned an invalid response for " + userID);
                    }
                }
                else
                {
                    m_log.Warn("[PRESENCE CONNECTOR]: Failed to retrieve sessions for " + userID + ": " + response["Message"].AsString());
                }
            }

            return presences;
        }

        private PresenceInfo ResponseToPresenceInfo(OSDMap sessionResponse, OSDMap userResponse)
        {
            if (sessionResponse == null)
                return null;

            PresenceInfo info = new PresenceInfo();

            info.Online = true;
            info.UserID = sessionResponse["UserID"].AsUUID().ToString();
            info.RegionID = sessionResponse["SceneID"].AsUUID();
            info.Position = sessionResponse["ScenePosition"].AsVector3();
            info.LookAt = sessionResponse["SceneLookAt"].AsVector3();

            if (userResponse != null && userResponse["User"] is OSDMap)
            {
                OSDMap user = (OSDMap)userResponse["User"];

                info.Login = user["LastLoginDate"].AsDate();
                info.Logout = user["LastLogoutDate"].AsDate();
                info.HomeRegionID = user["HomeSceneID"].AsUUID();
                info.HomePosition = user["HomePosition"].AsVector3();
                info.HomeLookAt = user["HomeLookAt"].AsVector3();
            }

            return info;
        }

        #endregion Helpers
    }
}
