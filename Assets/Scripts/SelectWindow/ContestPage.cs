using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Text.RegularExpressions;
using System;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class ContestPage : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI firstPrize;
    [SerializeField] public TextMeshProUGUI secondPrize;
    [SerializeField] public TextMeshProUGUI thirdPrize;
    [SerializeField] public TextMeshProUGUI entryFee;
    [SerializeField] public TextMeshProUGUI playerJoined;
    [SerializeField] public TextMeshProUGUI contestTime;

    public string contestId { get; set; }
    public string Id { get; set; }
    public int noOfuser { get; set; }
    public int joined { get; set; }
    public DateTime createdAt { get; set; }
    public DateTime updatedAt { get; set; }
    public int _firstPrize { get; set; }
    public int _secondPrize { get; set; }
    public int _thirdPrize { get; set; }
    public int _entryFee { get; set; }



    public void SetPrize(int prize)
    {
        firstPrize.text = prize.ToString();
    }
    public void SetFee(int prize)
    {
        entryFee.text = /*"Entry fee : $"+*/prize.ToString();
    }
    public void SetPlayerJOined()
    {
        playerJoined.text = joined.ToString();// + "/"+ noOfuser+ " Joined";
    }

    public void OnJoining()
    {
        DataSaver.Instance.contestId = Id;
        DataSaver.Instance.contestIdJoined = Id;
        ContestJoined();
        GameObject.FindGameObjectWithTag("content").transform.GetComponent<DisablePage>().MoveNext(false);
    }
    private void Update()
    {
        DateTime currentDateTime = DateTime.Now;
        double timeInActive = (currentDateTime - createdAt).TotalSeconds;
        int hour = (int)(timeInActive / 3600);
        int min = (int)((timeInActive % 3600) / 60);
        int sec = (int)((timeInActive % 3600) % 60);
        //Debug.Log("current DateTime -" + timeInActive );
        contestTime.text = min.ToString() + "m " + sec.ToString() + "s";
    }


    private string url = "https://ludo-project-backend.vercel.app/api/v1/user/joinContest/";
    public class Data
    {
        public int status;
        public string message;
    }

    public void ContestJoined()
    {
       // addWallet((-1) * _entryFee);
        Debug.Log("contest " + DataSaver.Instance.contestIdJoined);
        url = url + DataSaver.Instance.contestIdJoined;
        Debug.Log(url);
        StartCoroutine(Registrations(url));
    }

    IEnumerator Registrations(string url)
    {

        using (UnityWebRequest request = UnityWebRequest.Post(url,""))
        {
            /* byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

             request.uploadHandler = new UploadHandlerRaw(bodyRaw);*/
         
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + DataSaver.Instance.token) ;

            yield return request.SendWebRequest();
            var response = request.result;
            try
            {
                DataSaver.Instance.firstPrize = _firstPrize;
                DataSaver.Instance.secondPrize = _secondPrize;
                DataSaver.Instance.thirdPrize = _thirdPrize;
                DataSaver.Instance.entryFee = _entryFee;
                DataSaver.Instance.contestId = Id;
                addWallet((-1) * _entryFee);
                if (request.result != UnityWebRequest.Result.Success) Debug.Log(request.error);
                else if (request.result == UnityWebRequest.Result.Success)
                {
                    print("Successfully joined ");
                    var json = request.downloadHandler.text;
                    Debug.Log(json.ToString());
                  
                    Data val = JsonConvert.DeserializeObject<Data>(json.ToString());
                    Debug.Log("val.data.otp" + val.message);
                }
            }
            catch (Exception e)
            {
                print("exception " + e);
            }
            finally
            {

            }
        }

    }


    private string url1 = "https://ludo-project-backend.vercel.app/api/v1/user/wallet/addWallet";
    public void addWallet(int bal)
    {
        // url = url + DataSaver.Instance.contestIdJoined;
        StartCoroutine(Registrations1(url1, bal));
    }

    IEnumerator Registrations1(string url, int balance)
    {
        string jsonData = $"{{\"balance\": \"{balance}\"}}";
        Debug.Log(jsonData+ DataSaver.Instance.token);
        // Validate the data fields before sending the request
        if (!string.IsNullOrEmpty(jsonData))
        {
            using (UnityWebRequest request = UnityWebRequest.Post(url, ""))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);

                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                request.SetRequestHeader("Authorization", "Bearer " + DataSaver.Instance.token);

                yield return request.SendWebRequest();
                var response = request.result;
                try
                {
                    if (request.result != UnityWebRequest.Result.Success) Debug.Log(request.error);
                    else if (request.result == UnityWebRequest.Result.Success)
                    {
                        print("Successfully addwallet ");
                        var json = request.downloadHandler.text;
                        Debug.Log(json.ToString());

                        //myData val = JsonConvert.DeserializeObject<myData>(json.ToString());

                    }
                }
                catch (Exception e)
                {
                    print(e);
                }
                finally
                {

                }
            }
        }
    }

}
