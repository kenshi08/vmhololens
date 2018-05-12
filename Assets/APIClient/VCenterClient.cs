using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CI.HttpClient;
using vapitypes;
using System;
using System.Net;
#if !NETFX_CORE
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
#endif

public class VCenterClient
{
    string hostUrl, username, password;
    HttpClient httpClient;
    public VCenterClient(string hostUrl, string username, string password)
    {
        this.hostUrl = hostUrl;
        this.username = username;
        this.password = password;

        httpClient = new HttpClient();

#if !NETFX_CORE
        //allow custom certificate chains 
        ServicePointManager.ServerCertificateValidationCallback += (o, cert, chain, errors) =>
        {
            // all Certificates are accepted (potential security risk, should be revisited once the server starts sending proper certs)
            return true;
        };
#endif
    }

    public void Authenticate(Action<Exception,bool> callback)
    {
        var url = this.hostUrl + "/rest/com/vmware/cis/session/";
        Debug.Log("Requesting " + url);

        string encoded = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
        StringContent stringContent = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        httpClient.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " + encoded;
        httpClient.Post(new Uri(url), stringContent, response =>
         {
             if(response.IsSuccessStatusCode)
             {
                 //all ok
                 //set header for subsequent requests. The vcenter API uses cookies for session management
                 var cookies = response.OriginalResponse.Headers["Set-Cookie"];
                 foreach (var cookie in cookies.Split(new char[] { ';' }))
                 {
                     //we are interested in the vmware session id only
                     if (cookie.StartsWith("vmware"))
                     {
                         httpClient.CustomHeaders["Cookie"] = cookie;
                         break;
                     }
                 }

                 callback(null, true);

             } else
             {
                 var exception = new Exception("Authentication failed");
                 callback(exception,false);
             }
             Debug.Log(response.Data);

             //clear the auth header for future requests
             httpClient.Headers.Remove(System.Net.HttpRequestHeader.Authorization);
         });
    }

    public void ListClusters(Action<Exception, ClusterList> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/cluster";
        Debug.Log("Requesting " + url);
        httpClient.GetString(new Uri(url), response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null, JsonUtility.FromJson<ClusterList>(response.Data));
            } else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode), null);
            }
        });
    }

    public void ListHostsForCluster(string cluster, Action<Exception, HostList> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/host?filter.clusters="+cluster;
        Debug.Log("Requesting " + url);
        httpClient.GetString(new Uri(url), response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null, JsonUtility.FromJson<HostList>(response.Data));
            }
            else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode), null);
            }
        });
    }

    public void ListVmsForHost(string host, Action<Exception, VmList> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/vm?filter.hosts=" + host;
        Debug.Log("Requesting " + url);
        httpClient.GetString(new Uri(url), response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null, JsonUtility.FromJson<VmList>(response.Data));
            }
            else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode), null);
            }
        });
    }

    public void ListSingleVmForHost(string host, string vm, Action<Exception, VmList> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/vm?filter.hosts=" + host + "&filter.vms=" + vm;
        Debug.Log("Requesting " + url);
        httpClient.GetString(new Uri(url), response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null, JsonUtility.FromJson<VmList>(response.Data));
            }
            else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode), null);
            }
        });
    }

    public void StartVM(string vmIdentifier, Action<Exception> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/vm/" + vmIdentifier + "/power/start";
        Debug.Log("Requesting " + url);
        StringContent stringContent = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        httpClient.Post(new Uri(url), stringContent, response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null);
            }
            else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode));
            }
        });
    }

    public void StopVM(string vmIdentifier, Action<Exception> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/vm/" + vmIdentifier + "/power/stop";
        Debug.Log("Requesting " + url);
        StringContent stringContent = new StringContent("{}", System.Text.Encoding.UTF8, "application/json");
        httpClient.Post(new Uri(url), stringContent, response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null);
            }
            else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode));
            }
        });
    }

    public void DeleteVM(string vmIdentifier, Action<Exception> callback)
    {
        var url = this.hostUrl + "/rest/vcenter/vm/" + vmIdentifier;
        Debug.Log("Requesting " + url);
        
        httpClient.Delete(new Uri(url), response =>
        {
            Debug.Log(response.Data);
            if (response.IsSuccessStatusCode)
            {
                callback(null);
            }
            else
            {
                callback(new Exception("Something went wrong - " + response.StatusCode));
            }
        });
    }
}
