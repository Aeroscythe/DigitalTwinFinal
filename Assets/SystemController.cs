using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;

namespace M2MqttUnity.Examples
{
    public class SystemController : M2MqttUnityClient
    {

        public List<GameObject> controlledObjects = new List<GameObject>();

        public bool encryptedToggle;
        public string addressInputField;
        public string portInputField;

        private List<string> eventMessages = new List<string>();

        public void TestPublish()
        {
            client.Publish("M2MQTT_Unity/test", System.Text.Encoding.UTF8.GetBytes("Test message"), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
            Debug.Log("Test message published");
        }

        protected override void OnConnecting()
        {
            base.OnConnecting();
            Debug.Log("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();
            Debug.Log("Connected to broker on " + brokerAddress + "\n");
        }

        protected override void SubscribeTopics()
        {
            client.Subscribe(new string[] { "M2MQTT_Unity/test" }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });
        }

        protected override void UnsubscribeTopics()
        {
            client.Unsubscribe(new string[] { "M2MQTT_Unity/test" });
        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
        }

        protected override void OnDisconnected()
        {
            Debug.Log("Disconnected.");
        }

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }



        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }

        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            StoreMessage(msg);
        }

        private void StoreMessage(string eventMsg)
        {
            eventMessages.Add(eventMsg);
        }

        private void ProcessMessage(string msg)
        {
            Debug.Log("Received: " + msg);
        }



        // Update is called once per frame
        protected override void Update()
        {

            base.Update(); // call ProcessMqttEvents()

            if (eventMessages.Count > 0)
            {
                foreach (string msg in eventMessages)
                {
                    ProcessMessage(msg);
                }
                eventMessages.Clear();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                foreach (GameObject obj in controlledObjects)
                {
                    SwitchObjectState(obj);
                    Debug.Log("Something 2");
                }
                SendStateData();
            }

            if (Input.GetKeyDown(KeyCode.RightShift))
            {
                foreach (GameObject obj in controlledObjects)
                {
                    CloseEverything();
                    Debug.Log("Something 2");
                }
                SendStateData();
            }
        }

        private void OnDestroy()
        {
            Disconnect();
        }

        public void SwitchObjectState(GameObject obj)
        {
            obj.GetComponent<Appliance>().flipState();
            Debug.Log("something 3");
        }

        public void SendStateData()
        {
            foreach (GameObject obj in controlledObjects)
            {
                obj.GetComponent<Appliance>().ReadState();
                Debug.Log("State Read");
            }
        }

        public void CloseEverything()
        {
            foreach (GameObject obj in controlledObjects)
            {
                obj.GetComponent<Appliance>().WriteState(false);
            }
        }
    }
}
