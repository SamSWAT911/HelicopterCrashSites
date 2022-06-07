using EFT.Interactive;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace SamSWAT.HeliCrash
{
    public class HeliCrash : MonoBehaviour
    {
        private AssetBundle UH60Bundle;
        private GameObject Choppa;
        private Location HeliLocation;
        private LootableContainer ChoppaContainer;
        private string playerlocation;
        //public GameObject fpscamera;
        //private int counter = 0;

        public async void Init(string location)
        {
            playerlocation = location;
            string path = Plugin.Directory + "Assets/Content/Vehicles/sikorsky_uh60_blackhawk.bundle";

            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(path);

            while (!bundleLoadRequest.isDone)
                await Task.Yield();

            UH60Bundle = bundleLoadRequest.assetBundle;

            if (UH60Bundle == null)
            {
                Debug.LogError("[SamSWAT.HeliCrash]: Can't load UH-60 Blackhawk bundle");
                return;
            }

            var assetLoadRequest = UH60Bundle.LoadAllAssetsAsync<GameObject>();

            while (!assetLoadRequest.isDone)
                await Task.Yield();

            Choppa = assetLoadRequest.allAssets[0] as GameObject;

            HeliLocation = HeliCrashLocation();
            Choppa = Instantiate(Choppa, HeliLocation.Position, Quaternion.Euler(HeliLocation.Rotation));
            UH60Bundle.Unload(false);

            ChoppaContainer = Choppa.GetComponentInChildren<LootableContainer>();
            var airdropsyncobjects = LocationScene.GetAll<SynchronizableObject>().Where(x => x.GetComponent<AirdropSynchronizableObject>());
            var airdropcrates = new List<LootableContainer>();

            foreach(var crate in airdropsyncobjects)
            {
                airdropcrates.Add(crate.GetComponentInChildren<LootableContainer>());
            }

            ChoppaContainer.ItemOwner = PickRandom(airdropcrates).ItemOwner;
            //fpscamera = GameObject.Find("FPS Camera");
        }

        private Location HeliCrashLocation()
        {
            switch (playerlocation)
            {
                case "bigmap":
                    {
                        return PickRandom(Plugin.heliCrashSites.Customs);
                    }
                case "Interchange":
                    {
                        return PickRandom(Plugin.heliCrashSites.Interchange);
                    }
                case "RezervBase":
                    {
                        return PickRandom(Plugin.heliCrashSites.Rezerv);
                    }
                case "Shoreline":
                    {
                        return PickRandom(Plugin.heliCrashSites.Shoreline);
                    }
                case "Woods":
                    {
                        return PickRandom(Plugin.heliCrashSites.Woods);
                    }
                case "Lighthouse":
                    {
                        return PickRandom(Plugin.heliCrashSites.Lighthouse);
                    }
                case "develop":
                    {
                        return PickRandom(Plugin.heliCrashSites.Develop);
                    }
                default: return new Location();
            }
        }

        private T PickRandom<T>(List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }

        /* private void Update()
         {
             if (Input.GetKeyDown(KeyCode.Keypad5))
             {
                 var cumpos = fpscamera.transform.position;
                 var cumrot = fpscamera.transform.eulerAngles;
                 Physics.Raycast(cumpos, Vector3.down, out var raycastHit, 50f);
                 Choppa.transform.position = new Vector3(cumpos.x, raycastHit.point.y - 0.12f, cumpos.z);
                 Choppa.transform.eulerAngles = new Vector3(0, cumrot.y, 0);
             }
             if (Input.GetKeyDown(KeyCode.Keypad6))
             {
                 switch (playerlocation)
                 {
                     case "bigmap":
                         {
                             Plugin.heliCrashSites.Customs.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                     case "Interchange":
                         {
                             Plugin.heliCrashSites.Interchange.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                     case "RezervBase":
                         {
                             Plugin.heliCrashSites.Rezerv.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                     case "Shoreline":
                         {
                             Plugin.heliCrashSites.Shoreline.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                     case "Woods":
                         {
                             Plugin.heliCrashSites.Woods.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                     case "Lighthouse":
                         {
                             Plugin.heliCrashSites.Lighthouse.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                     case "develop":
                         {
                             Plugin.heliCrashSites.Develop.Add(new Location { Position = Choppa.transform.position, Rotation = Choppa.transform.rotation.eulerAngles });
                             break;
                         }
                 }
             }
             if (Input.GetKeyDown(KeyCode.Keypad0))
             {
                 File.WriteAllText(Plugin.Directory + "HeliCrashLocations.json", JsonConvert.SerializeObject(Plugin.heliCrashSites));
                 Debug.LogError("Data has been saved to file");
             }
             if (Input.GetKeyDown(KeyCode.Keypad8))
             {
                 switch (playerlocation)
                 {
                     case "bigmap":
                         {
                             if (counter < Plugin.heliCrashSites.Customs.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Customs[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Customs[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                     case "Interchange":
                         {
                             if (counter < Plugin.heliCrashSites.Interchange.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Interchange[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Interchange[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                     case "RezervBase":
                         {
                             if (counter < Plugin.heliCrashSites.Rezerv.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Rezerv[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Rezerv[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                     case "Shoreline":
                         {
                             if (counter < Plugin.heliCrashSites.Shoreline.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Shoreline[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Shoreline[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                     case "Woods":
                         {
                             if (counter < Plugin.heliCrashSites.Woods.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Woods[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Woods[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                     case "Lighthouse":
                         {
                             if (counter < Plugin.heliCrashSites.Lighthouse.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Lighthouse[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Lighthouse[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                     case "develop":
                         {
                             if (counter < Plugin.heliCrashSites.Develop.Count)
                             {
                                 var loca = Plugin.heliCrashSites.Develop[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             else
                             {
                                 counter = 0;
                                 var loca = Plugin.heliCrashSites.Develop[counter];
                                 Choppa.transform.position = loca.Position;
                                 Choppa.transform.eulerAngles = loca.Rotation;
                                 counter++;
                             }
                             break;
                         }
                 }
             }
         }*/
    }
}
