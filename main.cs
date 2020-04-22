using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using System.Linq;
using System.IO;
using System.Windows;
using System.Web;
using TMPro;

public class controller2
{
    int n_d_simple = 22;
    int n_d_comp = 20;
    String[] array;
    public Boolean count;
    entity2 ent = new entity2(true);
   // bool bol_er;
   // bool bol_ok;
    public void crea_ordinamento()
    {

        int i_d_simple = 1;
        int i_d_comp = 1;
        int n = n_d_comp + n_d_simple;
        array = new string[n + 1];
        array[0] = "vuoto";
        for (int i = 1; i < n + 1; ++i)
        {
            if (i_d_simple > n_d_simple && i_d_comp <= n_d_comp)
            {
                array[i] = "comp-" + i_d_comp;
                i_d_comp = i_d_comp + 1;
                continue;
            }
            if (i_d_simple <= n_d_simple && i_d_comp > n_d_comp)
            {
                array[i] = "simple-" + i_d_simple;
                i_d_simple = i_d_simple + 1;
                continue;
            }
            if (i_d_simple <= n_d_simple && i_d_comp <= n_d_comp)
            {
                if (((i % 2) == 0))
                {
                    array[i] = "comp-" + i_d_comp;
                    i_d_comp = i_d_comp + 1;
                }
                else
                {
                    array[i] = "simple-" + i_d_simple;
                    i_d_simple = i_d_simple + 1;
                }
            }

        }
        for (int i = 0; i < n + 1; ++i)
        {
            Debug.Log(array[i]);
        }
    }
   
    public controller2()
    {
        crea_ordinamento();
       


    }
    public void salva_partita()
    {
        // 0 true 1 false
        Debug.Log("salvamento:::" + PlayerPrefs.GetInt("salva"));
        PlayerPrefs.SetInt("bol_er", 0);
        PlayerPrefs.SetInt("bol_ok", 0);
        /*bol_er = true;
        bol_ok = true;*/
        if ((GameObject.Find("bet(Clone)") != null))
        {
            PlayerPrefs.SetInt("bol_ok", 1);
           // bol_ok = false;
        }
        if (GameObject.Find("final(Clone)") != null)
        {
            PlayerPrefs.SetInt("bol_er", 1);
          //  bol_er = false;
        }
       
        view2 v = new view2();
       int points =  v.rimuovi_punteggio();
        PlayerPrefs.SetInt("punteggio_salvato", points);
        v.pulisci_final();
        v.pulisci_bet();
        v.pulisci_domanda();
        v.torna_main_menu(this);
        GameObject gameObject = (GameObject)GameObject.Find("riprendi");
        MonoBehaviour.Destroy(gameObject);

        GameObject gameObject2 = Resources.Load<GameObject>("riprendi");
        UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(gameObject2) as UnityEngine.GameObject;
        tempTextBox6.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Riprendi partita";
        gameObject.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(delegate {
            if ((PlayerPrefs.GetInt("bol_ok") == 0) && (PlayerPrefs.GetInt("bol_er") == 1))
            {
                PlayerPrefs.SetInt("salva", 2);
            }
            riprendi_partita();
        });
        tempTextBox6.transform.SetParent(GameObject.Find("Panel").transform);
        tempTextBox6.transform.name = "riprendi";
        tempTextBox6.transform.Translate(new Vector3(1000f, 550f, 0));


        

    }
    public void riprendi_partita()
    {
       
        if (PlayerPrefs.GetInt("punteggio_salvato",-5) != -5)
        {
            int salva = PlayerPrefs.GetInt("salva");
            view2 v = new view2();
            v.istanzia_punteggio();
            GameObject gameObject = (GameObject)GameObject.Find("punt(Clone)");
            gameObject.transform.GetChild(0).GetComponent<Text>().text = "" + PlayerPrefs.GetInt("punteggio_salvato");
            GameObject gameObject2 = (GameObject)GameObject.Find("riprendi");
            MonoBehaviour.Destroy(gameObject2);

             gameObject2 = Resources.Load<GameObject>("riprendi");
            UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(gameObject2) as UnityEngine.GameObject;
            tempTextBox6.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Salva ed esci";
            tempTextBox6.transform.GetComponent<Button>().onClick.RemoveAllListeners();
            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(delegate {
                salva_partita(); });
            tempTextBox6.transform.SetParent(GameObject.Find("Panel").transform);
            tempTextBox6.transform.name = "riprendi";
            tempTextBox6.transform.Translate(new Vector3(1000f, 550f, 0));
            v.pulisci(true);
            v.pulisci(false);

            int a = PlayerPrefs.GetInt("count_domanda");
          //  Debug.Log(bol_er + "    " + bol_ok);
            int bol_er = PlayerPrefs.GetInt("bol_er");
            int bol_ok = PlayerPrefs.GetInt("bol_ok");
            if ((bol_er == 0 )&&(bol_ok == 0))
            {
                if (a != 1)
                {
                    a -=  1;
                }
            }
            if ((bol_ok == 0)&&(bol_er == 1)) {
                PlayerPrefs.SetInt("salva",2);
                increase_count();
            }
            if ((bol_ok == 1) && (bol_er == 0))

            {
                increase_count();
                v.aggiorna_punteggio();
            }


                Debug.Log("dom:"+a);
           
            String ord = array[a];
            String[] arr = ord.Split('-');
            if (arr[0] == "comp")
            {
                int n_domanda = Int32.Parse(arr[1]);
                DomandaMovie domanda = ent.extract_movie(n_domanda);
                if (salva == 1)
                {
                    Debug.Log("oratrue");
                    PlayerPrefs.SetInt("salva", 1);

                   
                }
                else
                {
                    Debug.Log("orafalse");
                    PlayerPrefs.SetInt("salva", 2);

                }
                domanda.instanzia(this);
            }
            if (arr[0] == "simple")
            {
                int n_domanda = Int32.Parse(arr[1]);
                Question2 domanda = new Question2();
                domanda = ent.extract_random(n_domanda);
             
                if (salva == 1)

                {
                    Debug.Log("oratrue");
                    PlayerPrefs.SetInt("salva", 1);

                  
                }
                else
                {
                    Debug.Log("orafalse");
                    PlayerPrefs.SetInt("salva", 2);
                    // salva_punteggio(PlayerPrefs.GetInt("count_domanda"));
                   

                }

                v.show_question_point(domanda, entity2.points, this);

            }


            PlayerPrefs.SetInt("punteggio_salvato", -5);
        }
        
    }
    public void start_quiz_button(Boolean b)
    {
        PlayerPrefs.SetInt("count_domanda", 1);
        PlayerPrefs.SetInt("salva", 1);
        view2 v = new view2();
        v.show_initial(b, this);
    }
    void increase_count()
    {
        int a = PlayerPrefs.GetInt("count_domanda");
        PlayerPrefs.SetInt("count_domanda", a + 1);
    }
    public void go_on()
    {
        int a = PlayerPrefs.GetInt("count_domanda");
        int salva = PlayerPrefs.GetInt("salva");
        
        view2 v = new view2();

        if (a < array.Length)
        {
            String ord = array[a];
            String[] arr = ord.Split('-');
            if (arr[0] == "comp")
            {
                
                int n_domanda = Int32.Parse(arr[1]);
                DomandaMovie domanda = ent.extract_movie(n_domanda);
                increase_count();
                if (salva == 1)
                {
                    Debug.Log("oratrue");
                    PlayerPrefs.SetInt("salva", 1);
                    
                    v.aggiorna_punteggio();
                    v.pulisci_bet();
                }
                else
                {
                    Debug.Log("orafalse");
                    PlayerPrefs.SetInt("salva", 2);
                    //salva_punteggio(PlayerPrefs.GetInt("count_domanda"));
                    v.pulisci_final();
                    v.pulisci_bet();

                }


                domanda.instanzia(this);
              
            }
            if (arr[0] == "simple")
            {
                int n_domanda = Int32.Parse(arr[1]);
                Question2 domanda = new Question2();
                domanda = ent.extract_random(n_domanda);
                increase_count();
                if (salva == 1)
                  
                {
                    Debug.Log("oratrue");
                    PlayerPrefs.SetInt("salva", 1);
                   
                    v.aggiorna_punteggio();
                    v.pulisci_bet();
                }
                else
                {
                    Debug.Log("orafalse");
                    PlayerPrefs.SetInt("salva", 2);
                   // salva_punteggio(PlayerPrefs.GetInt("count_domanda"));
                    v.pulisci_final();
                    v.pulisci_bet();

                }

                v.show_question_point(domanda, entity2.points, this);
                
            }
            if ((a % 10) == 0)
            {
                GestoreAds g = new GestoreAds();
                g.mostra();
            }
        }
        else
        { 
            if (!cerca_punteggio(PlayerPrefs.GetInt("count_domanda")))
            {
                salva_punteggio(PlayerPrefs.GetInt("count_domanda"));
            }
            
            v.hai_finito(this);
            v.rimuovi_punteggio();
            GestoreAds g = new GestoreAds();
            g.mostra();
            //per ora niente 
        }
    }
    public void select_right()
    {
        
        view2 v = new view2();
        Debug.Log("true");
        UnityEngine.UI.Text bet = ent.extract_bet();
        v.show_betwen(bet, this);

    }
    public void salva_punteggio(int punteggio)
    {
        int n = PlayerPrefs.GetInt("n_punteggi");
        n = n + 1;
        PlayerPrefs.SetInt("n_punteggi",n);
       PlayerPrefs.SetInt("punteggio" + n, punteggio);
    }
    public void select_wrong()
    {
       
        Debug.Log("falso");
        int points;
        view2 v = new view2();
        PlayerPrefs.SetInt("salva", 2);
        points = v.get_punteggio();
        if(!cerca_punteggio(points))
        {
            salva_punteggio(points);
        }
            
       
        
       
        v.show_final(points, this);

    }
    public bool cerca_punteggio(int punteggio)
    {
        int[] array = get_classifica_ord();
        bool esiste = false;
        for (int i = 0; i < array.Length;++i)
        {
            if(array[i] == punteggio)
            {
                esiste = true;
            }
        }
        return esiste;
    }
    public void start()
    {
        PlayerPrefs.SetInt("salva", 1);
        Question2 domanda = new Question2();
        domanda = ent.extract_random(1);
        increase_count();
        view2 v = new view2();
        v.show_question_point(domanda, 0, this);
        v.istanzia_punteggio();
        ///
        GameObject gameObject2 = (GameObject)GameObject.Find("riprendi");
        MonoBehaviour.Destroy(gameObject2);

        gameObject2 = Resources.Load<GameObject>("riprendi");
        UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(gameObject2) as UnityEngine.GameObject;
        tempTextBox6.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Salva ed esci";
        tempTextBox6.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(delegate {
            salva_partita();
        });
        tempTextBox6.transform.SetParent(GameObject.Find("Panel").transform);
        tempTextBox6.transform.name = "riprendi";
        tempTextBox6.transform.Translate(new Vector3(1000f, 550f, 0));

    }
    public int[] selection_sort(int[] array , int dim)
    {
        int m;
        for( int k = 0;k < dim-1;++k)
        {
            m = k+1;
            for (int j = k+1; j < dim;++j)
            {
                if (array[j] < array[m])
                {
                    m = j;
                }
            }
            Debug.Log("kkk"+ array[m]);
            int temp = array[k + 1];
            array[k + 1] = array[m];
            array[m] = temp;
        }
        for (int i = 0; i < array.Length - 1;++i)
        {
          if (array[i] > array[i+1])
            {
                int temp = array[i + 1];
                array[i + 1] = array[i];
                array[i] = temp;
            }
           
        }
        return array;
    }
    

       public int[] get_classifica_ord()
         {
        int dim = PlayerPrefs.GetInt("n_punteggi");
        int[] array = new int[dim];
        for (int i = 0;i < dim;++i)
        {
            int a = i + 1;
            array[i] = PlayerPrefs.GetInt("punteggio" + a);
        }
        array = selection_sort(array, dim);
        return array;

         }
       public void start_classifica(Boolean b) {
        int[] array = get_classifica_ord();
        view2 v = new view2();
        v.mostra_classifica(array,b);
        }

    }
public class view2

{
    public void mostra_classifica(int[] array,Boolean b)
    {
        pulisci(b);
        GameObject classifica = Resources.Load<GameObject>("classifica");
        GameObject panel = (GameObject) GameObject.Find("Panel");
        UnityEngine.GameObject tempTextBox = MonoBehaviour.Instantiate(classifica) as UnityEngine.GameObject;
        tempTextBox.transform.SetParent(panel.transform);
        tempTextBox.transform.Translate(new Vector3(1100f, 450f, 0));
        int dim = array.Length;
        GameObject boottone;
        if (dim == 8)
        {
            for (int i = dim - 1; i >= 0; --i)
            {
                int u = (dim - i);

                boottone = (GameObject)GameObject.Find("" + u);
                boottone.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = u + ")  " + array[i];

            }
        }
        if (dim > 8)
        {

        
        for (int i = dim -1;i >= 0;--i)
        {
            if (i > (dim - 8 - 1))
            {
                int u = (dim - i);

                boottone = (GameObject)GameObject.Find("" +u );
                boottone.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text =u+")  "+array[i] ;

            }
            else
            {
                break;
            }
        }
        }
        if (dim < 8)
        {
            for (int i = dim - 1; i >= 0; --i)
            {
                int u = (dim - i);

                    boottone = (GameObject)GameObject.Find("" + u);
                    boottone.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = u + ")  " + array[i];
 
            }
            for ( int i = dim+1;i <= 8;++i)
            {
                 boottone = (GameObject)GameObject.Find(""+i);
                boottone.transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>().text = "----";
            }
        }

        //mostra
    }
   
    public void istanzia_punteggio()
    {
        GameObject bottone = Resources.Load<GameObject>("punt");
        GameObject canvas = (GameObject)GameObject.Find("Canvas");

        GameObject tempTextBox2 = MonoBehaviour.Instantiate(bottone) as GameObject;
        tempTextBox2.transform.SetParent(canvas.transform, false);
        tempTextBox2.transform.Translate(new Vector3(0, 135f, 0));
        tempTextBox2.transform.GetChild(0).transform.Translate(new Vector3(20f, 0, 0));
    }
    public void aggiorna_punteggio()
    {
        GameObject punt = (GameObject)GameObject.Find("punt(Clone)");
        int a = Int32.Parse(punt.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text);
        a = a + 1;
        punt.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "" + a;

    }
    public int get_punteggio()
    {
        GameObject punt = (GameObject)GameObject.Find("punt(Clone)");
        int a = Int32.Parse(punt.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text);
        return a;
    }
    public int  rimuovi_punteggio()
    {
        GameObject punt = (GameObject)GameObject.Find("punt(Clone)");
        int a = Int32.Parse(punt.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text);
        MonoBehaviour.Destroy(punt);
        return a;
    }
    public void pulisci_domanda()
    {
        GameObject canvas = (GameObject)GameObject.Find("Canvas");

        GameObject canvas1 = (GameObject)GameObject.Find("testo");
        GameObject canvas2 = (GameObject)GameObject.Find("risposta1");
        GameObject canvas3 = (GameObject)GameObject.Find("risposta2");
        GameObject canvas4 = (GameObject)GameObject.Find("risposta3");
        GameObject canvas5 = (GameObject)GameObject.Find("risposta4");
        GameObject canvas6 = (GameObject)GameObject.Find("SampleWebView");
        GameObject canvas7 = (GameObject)GameObject.Find("status");
        GameObject canvas8 = (GameObject)GameObject.Find("WebViewObject");


        MonoBehaviour.Destroy(canvas1);
        MonoBehaviour.Destroy(canvas2);
        MonoBehaviour.Destroy(canvas3);
        MonoBehaviour.Destroy(canvas4);
        MonoBehaviour.Destroy(canvas5);
      if (canvas6 != null) {
            MonoBehaviour.Destroy(canvas6);
        }
        if (canvas7 != null)
        {
            MonoBehaviour.Destroy(canvas7);
        }
        if (canvas8 != null)
        {
            MonoBehaviour.Destroy(canvas8);
        }


       
       


    }
    public void show_betwen(UnityEngine.UI.Text bet,controller2 c)
    {
        this.pulisci_domanda();

        GameObject canvas = (GameObject)GameObject.Find("Canvas");

        GameObject bottone = Resources.Load<GameObject>("but");


        UnityEngine.UI.Text tempTextBox2 = MonoBehaviour.Instantiate(bet) as UnityEngine.UI.Text;
        tempTextBox2.transform.SetParent(canvas.transform, false);
        int a = PlayerPrefs.GetInt("count_domanda");

        UnityEngine.GameObject tempTextBox5 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox5.transform.SetParent(canvas.transform, false);
        tempTextBox5.transform.Translate(new Vector3(0f, -300f, 0));
        tempTextBox5.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = "Continua";
        tempTextBox5.transform.GetComponent<Button>().onClick.AddListener(delegate { c.go_on(); });

    }
    public void pulisci(Boolean b)
    {
        Debug.Log("devo andare");
       if (b)
        {
            GameObject canvas = (GameObject)GameObject.Find("but");
            GameObject canvas1 = (GameObject)GameObject.Find("but1");
            GameObject canvas2 = (GameObject)GameObject.Find("but2");
            MonoBehaviour.Destroy(canvas);
            MonoBehaviour.Destroy(canvas1);
            MonoBehaviour.Destroy(canvas2);

           
        }
        else
        {
            GameObject canvas3 = (GameObject)GameObject.Find("b1");
            MonoBehaviour.Destroy(canvas3);

            GameObject canvas4 = (GameObject)GameObject.Find("b2");
            MonoBehaviour.Destroy(canvas4);

            GameObject canvas5 = (GameObject)GameObject.Find("b3");

            MonoBehaviour.Destroy(canvas5);
        }
           

        
    }
    public void pulisci_bet()
    {
        GameObject bot_avvia = (GameObject)GameObject.Find("bet(Clone)");
        MonoBehaviour.Destroy(bot_avvia);
        GameObject bot_avvia2 = (GameObject)GameObject.Find("but(Clone)");
        MonoBehaviour.Destroy(bot_avvia2);


    }

    public void show_question_point(Question2 domanda, int points,controller2 c)
    {
        /* GameObject testo_iniziale = (GameObject)GameObject.Find("iniziale(Clone)");
         GameObject bot_avvia = (GameObject)GameObject.Find("but(Clone)");
         MonoBehaviour.Destroy(testo_iniziale);
         MonoBehaviour.Destroy(bot_avvia);*/
        GameObject initial = (GameObject)GameObject.Find("initial");
         MonoBehaviour.Destroy(initial); 


            GameObject canvas = (GameObject)GameObject.Find("Panel");

        GameObject bottone = Resources.Load<GameObject>("but2");
        GameObject testo = Resources.Load<GameObject>("testo_dom_semplice");


        String esatta = domanda.esatta;

        GameObject tempTextBox5 = MonoBehaviour.Instantiate(testo) as GameObject;
        tempTextBox5.transform.SetParent(canvas.transform, false);
        //tempTextBox5.transform.position = new Vector3(500f, 200f, 0f);
        tempTextBox5.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = domanda.testo;
        tempTextBox5.transform.name = "testo";

        UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox6.transform.SetParent(canvas.transform, false);
        tempTextBox6.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = domanda.risposta1;
        tempTextBox6.transform.name = "risposta1";
        tempTextBox6.transform.GetComponent<RectTransform>().position = new Vector3(-666f, -140, 0f);
        tempTextBox6.transform.Translate(new Vector3(1250f, 600f, 0));
        if (esatta == "1")
        {
            c.count = true;
            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "1")
        {
            c.count = false;

            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        UnityEngine.GameObject tempTextBox7 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox7.transform.SetParent(canvas.transform, false);
        tempTextBox7.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = domanda.risposta2;
        tempTextBox7.transform.name = "risposta2";
        tempTextBox7.transform.GetComponent<RectTransform>().position = new Vector3(-666f, -376f, 0f);
        tempTextBox7.transform.Translate(new Vector3(1250f, 600f, 0));
        if (esatta == "2")
        {
            c.count = true;
            tempTextBox7.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "2")
        {
            c.count = false;

            tempTextBox7.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        UnityEngine.GameObject tempTextBox8 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox8.transform.SetParent(canvas.transform, false);
        tempTextBox8.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = domanda.risposta3;
        tempTextBox8.transform.name = "risposta3";
        tempTextBox8.transform.GetComponent<RectTransform>().position = new Vector3(225f, -140f, 0f);
        tempTextBox8.transform.Translate(new Vector3(1250f, 600f, 0));
        if (esatta == "3")
        {
            c.count = true;
            tempTextBox8.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "3")
        {
            c.count = false;

            tempTextBox8.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        UnityEngine.GameObject tempTextBox9 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox9.transform.SetParent(canvas.transform, false);
        tempTextBox9.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = domanda.risposta4;
        tempTextBox9.transform.name = "risposta4";
        tempTextBox9.transform.position = new Vector3(225f, -376f, 0f);
        tempTextBox9.transform.Translate(new Vector3(1250f, 600f, 0));
        if (esatta == "4")
        {
            c.count = true;
            tempTextBox9.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "4")
        {
            c.count = false;

            tempTextBox9.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        int  a = PlayerPrefs.GetInt("count_domanda");
        Debug.Log("domanda:" + a);

    }
    public void pulisci_final()
    {
        GameObject bot_avvia = (GameObject)GameObject.Find("final(Clone)");
        MonoBehaviour.Destroy(bot_avvia);
        GameObject bot_avvia2 = (GameObject)GameObject.Find("but(Clone)");
        MonoBehaviour.Destroy(bot_avvia2);
        GameObject bot_avvia3 = (GameObject)GameObject.Find("bottone2");
        MonoBehaviour.Destroy(bot_avvia3);

    }
    public void torna_main_menu(controller2 c)
    {
        GameObject bot_avvia = (GameObject)GameObject.Find("final(Clone)");
        MonoBehaviour.Destroy(bot_avvia);
        GameObject bot_avvia2 = (GameObject)GameObject.Find("but(Clone)");
        MonoBehaviour.Destroy(bot_avvia2);
        GameObject bottone22 = (GameObject)GameObject.Find("bottone2");
        MonoBehaviour.Destroy(bottone22);
        GameObject canvas = (GameObject)GameObject.Find("Canvas");

        
        GameObject bottone1 = Resources.Load<GameObject>("but");
        UnityEngine.GameObject tempTextBox3 = MonoBehaviour.Instantiate(bottone1) as UnityEngine.GameObject;
        tempTextBox3.transform.SetParent(canvas.transform, false);
        tempTextBox3.transform.GetChild(0).transform.GetComponent<UnityEngine.UI.Text>().text = "Inizio quiz";
        tempTextBox3.name = "b1";
        tempTextBox3.transform.GetComponent<Button>().onClick.AddListener(delegate {
            c.start_quiz_button(false);
        });
        tempTextBox3.transform.position = new Vector3(970f, 600f, 0f);
        tempTextBox3.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = new Color(0, 0, 1, 1);
        GameObject bottone2 = Resources.Load<GameObject>("but");
        UnityEngine.GameObject tempTextBox4 = MonoBehaviour.Instantiate(bottone2) as UnityEngine.GameObject;
        tempTextBox4.transform.SetParent(canvas.transform, false);
        tempTextBox4.transform.GetChild(0).transform.GetComponent<UnityEngine.UI.Text>().text = "Classifica";
        tempTextBox4.transform.position = new Vector3(970f, 480f, 0f);
        tempTextBox4.name = "b2";
        tempTextBox4.transform.GetComponent<Button>().onClick.AddListener(delegate {
            c.start_classifica(false);
        });
        tempTextBox4.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = new Color(0, 0, 1, 1);
        GameObject bottone3 = Resources.Load<GameObject>("but");
        UnityEngine.GameObject tempTextBox5 = MonoBehaviour.Instantiate(bottone3) as UnityEngine.GameObject;
        tempTextBox5.transform.SetParent(canvas.transform, false);
        tempTextBox5.name = "b3";
        tempTextBox5.transform.GetChild(0).transform.GetComponent<UnityEngine.UI.Text>().text = "Esci";
        tempTextBox5.transform.position = new Vector3(970f, 360f, 0f);
        tempTextBox5.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().color = new Color(0, 0, 1, 1);
        tempTextBox5.transform.GetComponent<Button>().onClick.AddListener(delegate {
            Application.Quit();
        });

        GameObject gameObject = (GameObject)GameObject.Find("riprendi");
        MonoBehaviour.Destroy(gameObject);

        GameObject gameObject2 = Resources.Load<GameObject>("riprendi");
        UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(gameObject2) as UnityEngine.GameObject;
        tempTextBox6.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Riprendi partita";
        gameObject.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(delegate {
            c.riprendi_partita();
        });
        tempTextBox6.transform.SetParent(GameObject.Find("Panel").transform);
        tempTextBox6.transform.name = "riprendi";
        tempTextBox6.transform.Translate(new Vector3(1000f, 550f, 0));
    }
    public void hai_finito(controller2 c)
    {
        this.pulisci_bet();
        PlayerPrefs.SetInt("count_domanda", 1);
        GameObject canvas = (GameObject)GameObject.Find("Canvas");

        UnityEngine.UI.Text testo_iniziale = Resources.Load<UnityEngine.UI.Text>("final");
        GameObject bottone = Resources.Load<GameObject>("but");

        UnityEngine.UI.Text tempTextBox = MonoBehaviour.Instantiate(testo_iniziale) as UnityEngine.UI.Text;
        tempTextBox.transform.SetParent(canvas.transform, false);
        tempTextBox.transform.GetComponent<UnityEngine.UI.Text>().text ="Hai finito tutte le domande complimenti";


        UnityEngine.GameObject tempTextBox3 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox3.transform.SetParent(canvas.transform, false);
        tempTextBox3.transform.Translate(new Vector3(0f, -300f, 0));
        tempTextBox3.transform.GetChild(0).transform.GetComponent<UnityEngine.UI.Text>().text = "Menu principale";
        tempTextBox3.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            this.torna_main_menu(c);
        });
    }
    public void show_final(int points,controller2 c)
    {
        this.pulisci_domanda();
        GameObject canvas = (GameObject)GameObject.Find("Canvas");

        UnityEngine.UI.Text testo_iniziale = Resources.Load<UnityEngine.UI.Text>("final");
        GameObject bottone = Resources.Load<GameObject>("but");
        GameObject bottone2 = Resources.Load<GameObject>("but3");


        UnityEngine.UI.Text tempTextBox = MonoBehaviour.Instantiate(testo_iniziale) as UnityEngine.UI.Text;
        tempTextBox.transform.SetParent(canvas.transform, false);
        tempTextBox.transform.GetComponent<UnityEngine.UI.Text>().text = "Punteggio:   " + points + "    Hai perso";


        UnityEngine.GameObject tempTextBox3 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox3.transform.SetParent(canvas.transform, false);
        tempTextBox3.transform.Translate(new Vector3(-200f, -300f, 0));
        tempTextBox3.transform.GetChild(0).transform.GetComponent<UnityEngine.UI.Text>().text = "Menu principale";
        tempTextBox3.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            //PlayerPrefs.SetInt("count_domanda", 1);
            this.rimuovi_punteggio();
            this.torna_main_menu(c);
        });

        UnityEngine.GameObject tempTextBox4 = MonoBehaviour.Instantiate(bottone2) as UnityEngine.GameObject;
        tempTextBox4.transform.SetParent(canvas.transform, false);
        tempTextBox4.transform.Translate(new Vector3(200f, 0, 0));
        tempTextBox4.transform.name = "bottone2";
        tempTextBox4.transform.GetComponent<RectTransform>().localScale.Scale(new Vector3(2, 1, 1));
        tempTextBox4.transform.GetChild(0).transform.GetComponent<UnityEngine.UI.Text>().text = "Vai avanti lo stesso";
        tempTextBox4.transform.GetComponent<Button>().onClick.AddListener(delegate
        {
            PlayerPrefs.SetInt("salva", 2);
            c.go_on();
        });
    }

    public void show_initial(Boolean b,controller2 c)
    {
        this.pulisci(b);
        /* GameObject canvas = (GameObject)GameObject.Find("Panel");

         UnityEngine.UI.Text testo_iniziale = Resources.Load<UnityEngine.UI.Text>("iniziale");
         GameObject bottone = Resources.Load<GameObject>("but");

         UnityEngine.UI.Text tempTextBox = MonoBehaviour.Instantiate(testo_iniziale) as UnityEngine.UI.Text;
         tempTextBox.transform.SetParent(canvas.transform, false);

         UnityEngine.GameObject tempTextBox3 = UnityEngine.GameObject.Instantiate(bottone) as UnityEngine.GameObject;
         tempTextBox3.transform.SetParent(canvas.transform, false);
         tempTextBox3.transform.Translate(new Vector3(0f, -300f, 0f));
         tempTextBox3.transform.GetComponent<Button>().onClick.AddListener(delegate { c.start(); });*/

        GameObject canvas = (GameObject)GameObject.Find("Panel");
        GameObject bottone = Resources.Load<GameObject>("initial");
        GameObject tempTextBox = MonoBehaviour.Instantiate(bottone) as GameObject;
        tempTextBox.transform.name = "initial";
        tempTextBox.transform.SetParent(canvas.transform, false);
        tempTextBox.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { c.start(); });
    }
}



public class entity2
{
    public static int points = 0;
    public int n_questions = 2;
   
    public entity2(Boolean b)
    { 
            if (b == true)
        {
            String testo1 = "Dove si sono conosciuti Ficarra e Picone?";
            PlayerPrefs.SetString("testo1", testo1);
            PlayerPrefs.SetString("risp1-1", "Villaggio turistico");
            PlayerPrefs.SetString("risp2-1", "In un bar");
            PlayerPrefs.SetString("risp3-1", "In un cinema");
            PlayerPrefs.SetString("risp4-1", "Al teatro");
            PlayerPrefs.SetString("esa1", "1");

            String testo2 = "Qual'era il vecchio nome del trio?";
            PlayerPrefs.SetString("testo2", testo2);
            PlayerPrefs.SetString("risp1-2", "Noi due");
            PlayerPrefs.SetString("risp2-2", "I fantastici 2");
            PlayerPrefs.SetString("risp3-2", "Il bello e il brutto");
            PlayerPrefs.SetString("risp4-2", "Chiamata urbana urgente");
            PlayerPrefs.SetString("esa2", "4");

            String testo3 = "Con chi è sposato Ficarra";
            PlayerPrefs.SetString("testo3", testo3);
            PlayerPrefs.SetString("risp1-3", "Belen rodriguez");
            PlayerPrefs.SetString("risp2-3", "Rossella leone");
            PlayerPrefs.SetString("risp3-3", "Barbara d'urso");
            PlayerPrefs.SetString("risp4-3", "Ilary brasi");
            PlayerPrefs.SetString("esa3", "2");

            String testo4 = "Chi era il terzo componente del trio ?";
            PlayerPrefs.SetString("testo4", testo4);
            PlayerPrefs.SetString("risp1-4", "Salvo Borrello");
            PlayerPrefs.SetString("risp2-4", "Aldo baglio");
            PlayerPrefs.SetString("risp3-4", "Stefano Piazza");
            PlayerPrefs.SetString("risp4-4", "Alessandro piazza");
            PlayerPrefs.SetString("esa4", "1");

            String testo5 = "In quale fumetto noto apparvero i due?";
            PlayerPrefs.SetString("testo5", testo5);
            PlayerPrefs.SetString("risp1-5", "Zio paperone");
            PlayerPrefs.SetString("risp2-5", "One piece");
            PlayerPrefs.SetString("risp3-5", "Dragon balls");
            PlayerPrefs.SetString("risp4-5", "Naruto");
            PlayerPrefs.SetString("esa5", "1");

            String testo6 = "Come si chiama il loro libro?";
            PlayerPrefs.SetString("testo6", testo6);
            PlayerPrefs.SetString("risp1-6", "Io e lui");
            PlayerPrefs.SetString("risp2-6", "Chiamata urbana urgente");
            PlayerPrefs.SetString("risp3-6", "Diciamoci la verita");
            PlayerPrefs.SetString("risp4-6", "Il 7 e l'8");
            PlayerPrefs.SetString("esa6", "3");

            String testo7 = "Che piatto preparò Ficarra nel programma di Benedetta Parodi?";
            PlayerPrefs.SetString("testo7", testo7);
            PlayerPrefs.SetString("risp1-7", "Pizza");
            PlayerPrefs.SetString("risp2-7", "Hamburger");
            PlayerPrefs.SetString("risp3-7", "Matriciana");
            PlayerPrefs.SetString("risp4-7", "Pasta carne piselli e fragole");
            PlayerPrefs.SetString("esa7", "4");

            String testo8 = "In che facoltà si è laureato Picone?";
            PlayerPrefs.SetString("testo8", testo8);
            PlayerPrefs.SetString("risp1-8", "Ingegneria");
            PlayerPrefs.SetString("risp2-8", "Giurisprudenza");
            PlayerPrefs.SetString("risp3-8", "Economia");
            PlayerPrefs.SetString("risp4-8", "Lettere");
            PlayerPrefs.SetString("esa8", "2");

            String testo9 = "In che squadra gioca Picone?";
            PlayerPrefs.SetString("testo9", testo9);
            PlayerPrefs.SetString("risp1-9", "Misilmeri");
            PlayerPrefs.SetString("risp2-9", "Trapani");
            PlayerPrefs.SetString("risp3-9", "Villabate");
            PlayerPrefs.SetString("risp4-9", "Catania");
            PlayerPrefs.SetString("esa9", "1");

            String testo10 = "Con quale famoso duo tra questi hanno recitato?";
            PlayerPrefs.SetString("testo10", testo10);
            PlayerPrefs.SetString("risp1-10", "I soldi spicci");
            PlayerPrefs.SetString("risp2-10", "Mario e Paolo");
            PlayerPrefs.SetString("risp3-10", "Toti e Totino");
            PlayerPrefs.SetString("risp4-10", "Palla e Chiatta");
            PlayerPrefs.SetString("esa10", "3");

            String testo11 = "Stanco? rispondi come risponderebbe ficarra";
            PlayerPrefs.SetString("testo11", testo11);
            PlayerPrefs.SetString("risp1-11", "Si oggi si");
            PlayerPrefs.SetString("risp2-11", "Si tantissimo");
            PlayerPrefs.SetString("risp3-11", "Abbastanza");
            PlayerPrefs.SetString("risp4-11", "No oggi no");
            PlayerPrefs.SetString("esa11", "4");

            String testo12 = "In quale film i due sono stati scambiati alla nascita?";
            PlayerPrefs.SetString("testo12", testo12);
            PlayerPrefs.SetString("risp1-12", "Nati stanchi");
            PlayerPrefs.SetString("risp2-12", "Il 7 e l'8");
            PlayerPrefs.SetString("risp3-12", "Anche se è amore non si vede");
            PlayerPrefs.SetString("risp4-12", "L'ora legale");
            PlayerPrefs.SetString("esa12", "2");

            String testo13 = "Quale tra questi non è un film di Ficarra e Picone?";
            PlayerPrefs.SetString("testo13", testo13);
            PlayerPrefs.SetString("risp1-13", "Nati stanchi");
            PlayerPrefs.SetString("risp2-13", "Il 7 e l'8");
            PlayerPrefs.SetString("risp3-13", "Natale a Parigi");
            PlayerPrefs.SetString("risp4-13", "L'ora legale");
            PlayerPrefs.SetString("esa13", "3");

            String testo14 = "In che anno esce il loro film il 7 e l'8?";
            PlayerPrefs.SetString("testo14", testo14);
            PlayerPrefs.SetString("risp1-14", "2007");
            PlayerPrefs.SetString("risp2-14", "2006");
            PlayerPrefs.SetString("risp3-14", "2005");
            PlayerPrefs.SetString("risp4-14", "2016");
            PlayerPrefs.SetString("esa14", "1");

            String testo15 = "Quale tra questi è il loro primo film?";
            PlayerPrefs.SetString("testo15", testo15);
            PlayerPrefs.SetString("risp1-15", "Nati stanchi");
            PlayerPrefs.SetString("risp2-15", "Il 7 e l'8");
            PlayerPrefs.SetString("risp3-15", "La matassa");
            PlayerPrefs.SetString("risp4-15", "Anche se è amore non si vede");
            PlayerPrefs.SetString("esa15", "1");

            String testo16 = "In quale anno conducono per la prima volta striscia la notizia?";
            PlayerPrefs.SetString("testo16", testo16);
            PlayerPrefs.SetString("risp1-16", "2007");
            PlayerPrefs.SetString("risp2-16", "2008");
            PlayerPrefs.SetString("risp3-16", "2009");
            PlayerPrefs.SetString("risp4-16", "2005");
            PlayerPrefs.SetString("esa16", "4");

            String testo17 = "Quale tra queste non è una loro opera teatrale?";
            PlayerPrefs.SetString("testo17", testo17);
            PlayerPrefs.SetString("risp1-17", "Vuoti a  perdere");
            PlayerPrefs.SetString("risp2-17", "La matassa");
            PlayerPrefs.SetString("risp3-17", "Diciamoci la verita");
            PlayerPrefs.SetString("risp4-17", "Apriti cielo");
            PlayerPrefs.SetString("esa17", "2");

            String testo18 = "In quale film i due vanno da un capo mafia per chiedere uno sconto e una dilazione di pagamento ?";
            PlayerPrefs.SetString("testo18", testo18);
            PlayerPrefs.SetString("risp1-18", "Anche se è amore non si vede");
            PlayerPrefs.SetString("risp2-18", "Nati stanchi");
            PlayerPrefs.SetString("risp3-18", "Il 7 e l'8");
            PlayerPrefs.SetString("risp4-18", "La matassa");
            PlayerPrefs.SetString("esa18", "4");

            String testo19 = "In quale film Ficarra entra in casa sempre arrampicandosi dal balcone?";
            PlayerPrefs.SetString("testo19", testo19);
            PlayerPrefs.SetString("risp1-19", "Il 7 e l'8");
            PlayerPrefs.SetString("risp2-19", "Anche se è amore non si vede");
            PlayerPrefs.SetString("risp3-19", "Nati stanchi");
            PlayerPrefs.SetString("risp4-19", "La matassa");
            PlayerPrefs.SetString("esa19", "1");

            String testo20 = "In quale film sono cugini?";
            PlayerPrefs.SetString("testo20", testo20);
            PlayerPrefs.SetString("risp1-20", "La matassa");
            PlayerPrefs.SetString("risp2-20", "Nati stanchi");
            PlayerPrefs.SetString("risp3-20", "Anche se è amore non si vede");
            PlayerPrefs.SetString("risp4-20", "Il 7 e l'8");
            PlayerPrefs.SetString("esa20", "1");

            String testo21 = "In quale film Ficarra dice ad una ragazza 'Qui c'è una ferramenta tutta per te'?";
            PlayerPrefs.SetString("testo21", testo21);
            PlayerPrefs.SetString("risp1-21", "La matassa");
            PlayerPrefs.SetString("risp2-21", "Il 7 e l'8");
            PlayerPrefs.SetString("risp3-21", "Anche se è amore non si vede");
            PlayerPrefs.SetString("risp4-21", "Nati stanchi");
            PlayerPrefs.SetString("esa21", "3");

            String testo22 = "In quale film i due diventano biondi per sbaglio?";
            PlayerPrefs.SetString("testo22", testo22);
            PlayerPrefs.SetString("risp1-22", "La matassa");
            PlayerPrefs.SetString("risp2-22", "Il 7 e l'8");
            PlayerPrefs.SetString("risp3-22", "Anche se è amore non si vede");
            PlayerPrefs.SetString("risp4-22", "Nati stanchi");
            PlayerPrefs.SetString("esa22", "3");

            String Testo1 = "In questo video Ficarra e Picone fanno i progetti per realizzare : ?";
            PlayerPrefs.SetString("Testo1", Testo1);
            PlayerPrefs.SetString("Risp1-1", "Il ponte palermo genova");
            PlayerPrefs.SetString("Risp2-1", "Il ponte messina reggio calabria");
            PlayerPrefs.SetString("Risp3-1", "Un museo");
            PlayerPrefs.SetString("Risp4-1", "Un cinema");
            PlayerPrefs.SetString("Esa1", "2");

            String Testo2 = "Come si conclude questa scena ?";
            PlayerPrefs.SetString("Testo2", Testo2);
            PlayerPrefs.SetString("Risp1-2", "Finiscono in commissariato");
            PlayerPrefs.SetString("Risp2-2", "Riescono a prendere il treno");
            PlayerPrefs.SetString("Risp3-2", "Vengono sorpresi senza biglietto e scappano");
            PlayerPrefs.SetString("Risp4-2", "Cambiano idea e non prendono il treno");
            PlayerPrefs.SetString("Esa2", "1");

            String Testo3 = "Come si conclude questa scena ?";
            PlayerPrefs.SetString("Testo3", Testo3);
            PlayerPrefs.SetString("Risp1-3", "Scappano");
            PlayerPrefs.SetString("Risp2-3", "Fanno amicizia con il buttafuori");
            PlayerPrefs.SetString("Risp3-3", "Litigano con il buttafuori");
            PlayerPrefs.SetString("Risp4-3", "Arriva un altro buttafuori e litigano tutti e 4");
            PlayerPrefs.SetString("Esa3", "3");

            String Testo4 = "Qual'è il titolo dell'opera teatrale da cui è stato tratto questo video ?";
            PlayerPrefs.SetString("Testo4", Testo4);
            PlayerPrefs.SetString("Risp1-4", "Sono cose che capitano");
            PlayerPrefs.SetString("Risp2-4", "La matassa");
            PlayerPrefs.SetString("Risp3-4", "Il primo natale");
            PlayerPrefs.SetString("Risp4-4", "Il 7 e l'8");
            PlayerPrefs.SetString("Esa4", "1");

            String Testo5 = "Se hai visto questo sketch saprai com'è morto realmente, come? ";
            PlayerPrefs.SetString("Testo5", Testo5);
            PlayerPrefs.SetString("Risp1-5", "E stato ucciso");
            PlayerPrefs.SetString("Risp2-5", "Di vecchiaia");
            PlayerPrefs.SetString("Risp3-5", "In un incidente");
            PlayerPrefs.SetString("Risp4-5", "In un agguato");
            PlayerPrefs.SetString("Esa5", "2");

            String Testo6 = "In base al video che hai visto quanti anni fa è morto il padre dello zio di ficarra?";
            PlayerPrefs.SetString("Testo6", Testo6);
            PlayerPrefs.SetString("Risp1-6", "60");
            PlayerPrefs.SetString("Risp2-6", "30");
            PlayerPrefs.SetString("Risp3-6", "40");
            PlayerPrefs.SetString("Risp4-6", "50");
            PlayerPrefs.SetString("Esa6", "4");

            String Testo7 = "Picone è stato in viaggio in : ";
            PlayerPrefs.SetString("Testo7", Testo7);
            PlayerPrefs.SetString("Risp1-7", "Parigi");
            PlayerPrefs.SetString("Risp2-7", "Egitto");
            PlayerPrefs.SetString("Risp3-7", "Grecia");
            PlayerPrefs.SetString("Risp4-7", "Giappone");
            PlayerPrefs.SetString("Esa7", "2");

            String Testo8 = "Ficarra è stato in viaggio in ";
            PlayerPrefs.SetString("Testo8", Testo8);
            PlayerPrefs.SetString("Risp1-8", "Africa");
            PlayerPrefs.SetString("Risp2-8", "Svizzera");
            PlayerPrefs.SetString("Risp3-8", "Egitto");
            PlayerPrefs.SetString("Risp4-8", "Grecia");
            PlayerPrefs.SetString("Esa8", "2");

            String Testo9 = "Dove si è laureato il professor Ficazza?";
            PlayerPrefs.SetString("Testo9", Testo9);
            PlayerPrefs.SetString("Risp1-9", "Palermo");
            PlayerPrefs.SetString("Risp2-9", "Pisa");
            PlayerPrefs.SetString("Risp3-9", "Parigi");
            PlayerPrefs.SetString("Risp4-9", "Milano");
            PlayerPrefs.SetString("Esa9", "3");

            String Testo10 = "Hai sentito la lazio?";
            PlayerPrefs.SetString("Testo10", Testo10);
            PlayerPrefs.SetString("Risp1-10", "Piu di 110 miliardi per un giocatore");
            PlayerPrefs.SetString("Risp2-10", "no non ho sentito");
            PlayerPrefs.SetString("Risp3-10", "Piu di 2 miliardi per un giocatore");
            PlayerPrefs.SetString("Risp4-10", "Meno di 110 miliardi per un giocatore");
            PlayerPrefs.SetString("Esa10", "1");

            String Testo11 = "A quanto ammonta l'assegno di invalidità ?";
            PlayerPrefs.SetString("Testo11", Testo11);
            PlayerPrefs.SetString("Risp1-11", "1 milione");
            PlayerPrefs.SetString("Risp2-11", "4 milioni 450 mila lire");
            PlayerPrefs.SetString("Risp3-11", "3 milioni ");
            PlayerPrefs.SetString("Risp4-11", "7 milioni 832 mila lire");
            PlayerPrefs.SetString("Esa11", "4");

            String Testo12 = "Perche si finge straniero ?";
            PlayerPrefs.SetString("Testo12", Testo12);
            PlayerPrefs.SetString("Risp1-12", "Perche si vergogna di essere italiano");
            PlayerPrefs.SetString("Risp2-12", "Perche l'italia non gli piace");
            PlayerPrefs.SetString("Risp3-12", "Per giocare nell'inter");
            PlayerPrefs.SetString("Risp4-12", "Nessuna delle precedenti");
            PlayerPrefs.SetString("Esa12", "3");

            String Testo13 = "Per cosa vengono usati i cinesi che giocano nell'inter?";
            PlayerPrefs.SetString("Testo13", Testo13);
            PlayerPrefs.SetString("Risp1-13", "Tutti come attaccanti");
            PlayerPrefs.SetString("Risp2-13", "Tutti come difensori ");
            PlayerPrefs.SetString("Risp3-13", "Come pezzi di ricambio");
            PlayerPrefs.SetString("Risp4-13", "Tutti come centrocampisti");
            PlayerPrefs.SetString("Esa13", "3");

            String Testo14 = "Cosa ha comprato Picone per fare shopping?";
            PlayerPrefs.SetString("Testo14", Testo14);
            PlayerPrefs.SetString("Risp1-14", "Una ferrari e due BMW");
            PlayerPrefs.SetString("Risp2-14", "Due ferrari e una BMW");
            PlayerPrefs.SetString("Risp3-14", "Due ford e una ferrari ");
            PlayerPrefs.SetString("Risp4-14", "Due ferrari e due ford");
            PlayerPrefs.SetString("Esa14", "1");

            String Testo15 = "Non c'è cosa piu divina che...?";
            PlayerPrefs.SetString("Testo15", Testo15);
            PlayerPrefs.SetString("Risp1-15", "cominciare con la cugina");
            PlayerPrefs.SetString("Risp2-15", "festeggiare la nuova nascita");
            PlayerPrefs.SetString("Risp3-15", "fare un regalino alla bambina");
            PlayerPrefs.SetString("Risp4-15", "fare un regalino alla neomamma");
            PlayerPrefs.SetString("Esa15", "1");

            String Testo16 = "Con cosa Picone sarebbe l'uomo piu felice del mondo?";
            PlayerPrefs.SetString("Testo16", Testo16);
            PlayerPrefs.SetString("Risp1-16", "Con 300g di cacca");
            PlayerPrefs.SetString("Risp2-16", "Con 100g di cacca");
            PlayerPrefs.SetString("Risp3-16", "Con 1kg di cacca");
            PlayerPrefs.SetString("Risp4-16", "Con 150g di cacca");
            PlayerPrefs.SetString("Esa16", "2");

            String Testo17 = "Con quali parole Paola ha lasciato Valentino?";
            PlayerPrefs.SetString("Testo17", Testo17);
            PlayerPrefs.SetString("Risp1-17", "Valentino ho un altro");
            PlayerPrefs.SetString("Risp2-17", "Valentino non mi sento piu sicura");
            PlayerPrefs.SetString("Risp3-17", "Valentino non ti amo piu");
            PlayerPrefs.SetString("Risp4-17", "Valentino tu non sei piu lo stesso di prima");
            PlayerPrefs.SetString("Esa17", "2");

            String Testo18 = "Di cosa vuole parlare Ficarra ?";
            PlayerPrefs.SetString("Testo18", Testo18);
            PlayerPrefs.SetString("Risp1-18", "Di paola");
            PlayerPrefs.SetString("Risp2-18", "Di calcio");
            PlayerPrefs.SetString("Risp3-18", "Di Marcella");
            PlayerPrefs.SetString("Risp4-18", "Nessuna delle precedenti");
            PlayerPrefs.SetString("Esa18", "3");

            String Testo19 = "Quante femmine ha avuto Picone nei suoi giorni da single?";
            PlayerPrefs.SetString("Testo19", Testo19);
            PlayerPrefs.SetString("Risp1-19", "15");
            PlayerPrefs.SetString("Risp2-19", "20 + qualcuna in nero");
            PlayerPrefs.SetString("Risp3-19", "25");
            PlayerPrefs.SetString("Risp4-19", "22 + qualcuna in nero");
            PlayerPrefs.SetString("Esa19", "4");

            String Testo20 = "Carrisi attore americano?";
            PlayerPrefs.SetString("Testo20", Testo20);
            PlayerPrefs.SetString("Risp1-20", "Al");
            PlayerPrefs.SetString("Risp2-20", "Il");
            PlayerPrefs.SetString("Risp3-20", "Ale");
            PlayerPrefs.SetString("Risp4-20", "Mal");
            PlayerPrefs.SetString("Esa20", "1");

        }
            
    }
    public DomandaMovie extract_movie(int num_domanda)
    {
        String testo_v = "Testo" + num_domanda;
        String risposta1_v = "Risp1-" + num_domanda;
        String risposta2_v = "Risp2-" + num_domanda;
        String risposta3_v = "Risp3-" + num_domanda;
        String risposta4_v = "Risp4-" + num_domanda;
        String esa_v = "Esa" + num_domanda;
        String testo = PlayerPrefs.GetString(testo_v);
        String risposta1 = PlayerPrefs.GetString(risposta1_v);
        String risposta2 = PlayerPrefs.GetString(risposta2_v);
        String risposta3 = PlayerPrefs.GetString(risposta3_v);
        String risposta4 = PlayerPrefs.GetString(risposta4_v);
        String esa = PlayerPrefs.GetString(esa_v);
        String video = "video" + num_domanda;
        DomandaMovie domanda = new DomandaMovie(testo, risposta1, risposta2, risposta3, risposta4, video, esa);
        return domanda;
    }
    public Question2 extract_random(int num_domanda)
    {
        Debug.Log("::" + num_domanda);
        String testo_v = "testo" + num_domanda;
        String risposta1_v = "risp1-" + num_domanda;
        String risposta2_v = "risp2-" + num_domanda;
        String risposta3_v = "risp3-" + num_domanda;
        String risposta4_v = "risp4-" + num_domanda;
        String esa_v = "esa" + num_domanda;
       String testo=  PlayerPrefs.GetString(testo_v);
        String risposta1 = PlayerPrefs.GetString(risposta1_v);
        String risposta2 = PlayerPrefs.GetString(risposta2_v);
        String risposta3 = PlayerPrefs.GetString(risposta3_v);
       String risposta4 =  PlayerPrefs.GetString(risposta4_v);
        String esa = PlayerPrefs.GetString(esa_v);
        Question2 domanda = new Question2();
        domanda.set(testo, risposta1, risposta2, risposta3, risposta4, esa);

        return domanda;
    }

    public UnityEngine.UI.Text extract_bet()
    {
        UnityEngine.UI.Text bet = Resources.Load<UnityEngine.UI.Text>("bet");
        return bet;
    }

    public static int increase()
    {
        points++;
        return points;
    }
}
public class DomandaMovie
{
   
    public String testo;
    public String nome_video;
    UnityEngine.Video.VideoPlayer videoPlayer;
    RawImage rawImage;
    public String risposta1;
    public String risposta2;
    public String risposta3;
    public String risposta4;
    public String esatta;
   
    public void instanzia(controller2 c)
    {
        view2 v = new view2();
        v.pulisci_bet();
        /*
        RawImage tempTextBox1 = MonoBehaviour.Instantiate(rawImage) as RawImage;
        tempTextBox1.name = "Raw Image";
        tempTextBox1.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0));
        tempTextBox1.transform.Translate(new Vector3(1000f, 600f, 0));
        UnityEngine.Video.VideoPlayer tempTextBox2 = MonoBehaviour.Instantiate(videoPlayer) as UnityEngine.Video.VideoPlayer;
        tempTextBox2.name = "Video Player";
        tempTextBox2.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0));
        tempTextBox2.transform.Translate(new Vector3(1000f, 600f, 0));
        */
        GameObject gameObject = Resources.Load<GameObject>("SampleWebView");
        GameObject gameObject2 = Resources.Load<GameObject>("Status");
        GameObject tempTextBox = MonoBehaviour.Instantiate(gameObject) as GameObject;
        
        tempTextBox.name = "SampleWebView";
        tempTextBox.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0));
        tempTextBox.GetComponent<SampleWebView>().Url = nome_video + ".html";
        GameObject tempTextBox2 = MonoBehaviour.Instantiate(gameObject2) as GameObject;
        tempTextBox2.name = "status";
        tempTextBox2.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0));


        GameObject tempTextBox3 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("testo_dom_composta")) as GameObject;
        tempTextBox3.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = testo;
        tempTextBox3.name = "testo";
        tempTextBox3.transform.SetParent(GameObject.Find("Canvas").transform.GetChild(0));
        tempTextBox3.transform.Translate(new Vector3(1000f,600f,0));
        GameObject bottone = Resources.Load<GameObject>("but2");

        UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox6.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox6.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = risposta1;
        tempTextBox6.transform.name = "risposta1";
        tempTextBox6.transform.GetComponent<RectTransform>().position = new Vector3(617f, 44f, 0f);
        tempTextBox6.transform.Translate(new Vector3(900f, 550f, 0));
        if (esatta == "1")
        {
            c.count = true;
            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "1")
        {
            c.count = false;

            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        UnityEngine.GameObject tempTextBox7 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox7.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox7.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = risposta2;
        tempTextBox7.transform.name = "risposta2";
        tempTextBox7.transform.GetComponent<RectTransform>().position = new Vector3(617f, -119f, 0f);
        tempTextBox7.transform.Translate(new Vector3(900f, 550f, 0));
        if (esatta == "2")
        {
            c.count = true;
            tempTextBox7.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "2")
        {
            c.count = false;

            tempTextBox7.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        UnityEngine.GameObject tempTextBox8 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox8.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox8.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = risposta3;
        tempTextBox8.transform.name = "risposta3";
        tempTextBox8.transform.GetComponent<RectTransform>().position = new Vector3(617f, -285f, 0f);
        tempTextBox8.transform.Translate(new Vector3(900f, 550f, 0));
        if (esatta == "3")
        {
            c.count = true;
            tempTextBox8.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "3")
        {
            c.count = false;

            tempTextBox8.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        UnityEngine.GameObject tempTextBox9 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox9.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox9.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = risposta4;
        tempTextBox9.transform.name = "risposta4";
        tempTextBox9.transform.position = new Vector3(617f, -454f, 0f);
        tempTextBox9.transform.Translate(new Vector3(900f, 550f, 0));
        if (esatta == "4")
        {
            c.count = true;
            tempTextBox9.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "4")
        {
            c.count = false;

            tempTextBox9.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }
        /*UnityEngine.GameObject tempTextBox6 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox6.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox6.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = risposta1;
        tempTextBox6.name = "risposta1";
        tempTextBox6.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 20;
        tempTextBox6.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
        tempTextBox6.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
        tempTextBox6.transform.GetChild(0).transform.Translate(new Vector3(20f, -20f, 0));
        tempTextBox6.transform.name = "risposta1";
        if (risposta1.Length > 14)
        {
            tempTextBox6.transform.GetChild(0).transform.Translate(new Vector3(0, 20f, 0));
        }
        if (risposta1.Length >= 22)
        {
            tempTextBox6.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 15;
        }

        tempTextBox6.transform.position = new Vector3(1200f, 400f, 0f);
        if (esatta == "1")
        {
            c.count = true;
            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(c.select_right);

        }
        if (esatta != "1")
        {
            c.count = false;

            tempTextBox6.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }

        UnityEngine.GameObject tempTextBox8 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox8.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox8.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = risposta2;
        tempTextBox8.name = "risposta2";
        tempTextBox8.transform.position = new Vector3(1200f, 200f, 0f);
        tempTextBox8.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 20;
        tempTextBox8.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
        tempTextBox8.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
        tempTextBox8.transform.GetChild(0).transform.Translate(new Vector3(20f, -20f, 0));
        tempTextBox8.transform.name = "risposta2";
        if (risposta2.Length > 14)
        {
            tempTextBox8.transform.GetChild(0).transform.Translate(new Vector3(0, 20f, 0));
        }
        if (risposta2.Length >= 22)
        {
            tempTextBox8.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 15;
        }

        if (esatta == "2")
        {
            c.count = true;
            tempTextBox8.transform.GetComponent<Button>().onClick.AddListener(c.select_right);
        }
        if (esatta != "2")
        {
            c.count = false;

            tempTextBox8.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }



        UnityEngine.GameObject tempTextBox9 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox9.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox9.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = risposta3;
        tempTextBox9.transform.position = new Vector3(1600f, 400f, 0f);
        tempTextBox9.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 20;
        tempTextBox9.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
        tempTextBox9.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
        tempTextBox9.transform.GetChild(0).transform.Translate(new Vector3(20f, -20f, 0));
        tempTextBox9.transform.name = "risposta3";
        tempTextBox9.name = "risposta3";
        if (risposta3.Length > 14)
        {
            tempTextBox9.transform.GetChild(0).transform.Translate(new Vector3(0, 20f, 0));
        }
        if (risposta3.Length >= 22)
        {
            tempTextBox9.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 15;
        }
        if (esatta == "3")
        {
            c.count = true;
            tempTextBox9.transform.GetComponent<Button>().onClick.AddListener(c.select_right);
        }
        if (esatta != "3")
        {
            c.count = false;

            tempTextBox9.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }


        UnityEngine.GameObject tempTextBox10 = MonoBehaviour.Instantiate(bottone) as UnityEngine.GameObject;
        tempTextBox10.transform.SetParent(GameObject.Find("Canvas").transform, false);
        tempTextBox10.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().text = risposta4;
        tempTextBox10.transform.position = new Vector3(1600f, 200f, 0f);
        tempTextBox10.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 20;
        tempTextBox10.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 70);
        tempTextBox10.transform.GetChild(0).GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 180);
        tempTextBox10.transform.GetChild(0).transform.Translate(new Vector3(20f, -20f, 0));
        tempTextBox10.transform.name = "risposta4";
        tempTextBox10.name = "risposta4";
        if (risposta4.Length > 14)
        {
            tempTextBox10.transform.GetChild(0).transform.Translate(new Vector3(0, 20f, 0));
        }
        if (risposta4.Length >= 22)
        {
            tempTextBox10.transform.GetChild(0).GetComponent<UnityEngine.UI.Text>().fontSize = 15;
        }
        if (esatta == "4")
        {
            c.count = true;
            tempTextBox10.transform.GetComponent<Button>().onClick.AddListener(c.select_right);
        }
        if (esatta != "4")
        {
            c.count = false;
            tempTextBox10.transform.GetComponent<Button>().onClick.AddListener(c.select_wrong);
        }


        */

    }
    public DomandaMovie(String testo ,String r1,String r2,String r3,String r4 ,String nome_video,String esatta)
    {
        this.nome_video = nome_video;
        this.esatta = esatta;
        
        this.testo = testo;
        this.risposta1 = r1;
        this.risposta2 = r2;
        this.risposta3 = r3;
        this.risposta4 = r4;
        Debug.Log(":::::::"+nome_video);
    }

}
public class Question2
{
    public String  testo;
    public String risposta1;
    public String risposta2;
    public String risposta3;
    public String risposta4;
    public String esatta;
    public void set(String testo,String risposta1, String risposta2, String risposta3, String risposta4, String esatta)
    {
        this.testo = testo;
        this.risposta1 = risposta1;
        this.risposta2 = risposta2;
        this.risposta3 = risposta3;
        this.risposta4 = risposta4;
        this.esatta = esatta;

    }
}




public class main : MonoBehaviour
{

   

    public  void lancia_quiz()
    {
        Debug.Log("ciao");
        controller2 con = new controller2();
        con.start_quiz_button(true);
    }
    public void lancia_classifica()
    {
        controller2 con = new controller2();
        Debug.Log("ciao");
        con.start_classifica(true);
    }
    public void torna()
    {
        controller2 con = new controller2();
        Debug.Log("ciao");
        view2 v = new view2();
        GameObject classifica = (GameObject)GameObject.Find("classifica(Clone)");
        MonoBehaviour.Destroy(classifica);
        v.torna_main_menu(con);
    }
    public void riprendi()
    {
        controller2 con = new controller2();
        con.riprendi_partita();
    }
    public void esci()
    {
        Application.Quit();
    }
    // Start is called before the first frame update
    void Start()

    {
        GameObject ads = Resources.Load<GameObject>("ads");
        UnityEngine.GameObject tempTextBox = MonoBehaviour.Instantiate(ads) as UnityEngine.GameObject;
        if (PlayerPrefs.GetInt("n_punteggi",-5) == -5)
        {
            PlayerPrefs.SetInt("n_punteggi", 0);
        }
        Screen.SetResolution(2160, 1080, true);

        GestoreAds ad = new GestoreAds();
        ad.mostra();

        /* controller con = new controller();
         con.start_quiz_button();*/

    }
   

    // Update is called once per frame
    void Update()
    {
        

            
        
    }
} 