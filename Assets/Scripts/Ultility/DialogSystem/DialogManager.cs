﻿using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// One manager foreach Scene
/// </summary>
public class DialogManager : MonoSingleton<DialogManager>
{
    public DialogContainer dialogContainer;
    protected override void Awake()
    {
        base.Awake();
        _dicDialog = new Dictionary<string, BaseDialog>();
    }

    //TEST
    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         OnShowDialogWithTransition<BaseDialog>("Dialog/Home/Maps", "MainDialog");
    //     }
    // }
    
    public T OnShowDialogWithTransition<T>(string path, DialogType type, Action callback = null)
        where T : BaseDialog
    {
        var dialog = OnShowDialog<BaseDialog>(path, type,0.5f);
        if (dialog != null && !dialog.gameObject.activeSelf)
        {
            _transition.Play("Transition"); ;
            return dialog.GetComponent<T>();
        }

        return null;
    }

    public T OnShowDialog<T>(string path, DialogType type, float delayActive = 0, Action callback = null) where T : BaseDialog
    {
        if (!_dicDialog.ContainsKey(path))
        {
            GameObject dialogPrefab = LoaderUtility.Instance.GetAsset<GameObject>(path);
            if (dialogPrefab != null)
            {
                T newDialog = Instantiate(dialogPrefab.GetComponent<T>(), dialogContainer[type].trans);
                newDialog.transform.SetAsLastSibling();
                newDialog.gameObject.SetActive(false);
                StartCoroutine(DelayAction(delayActive, () =>
                {
                    newDialog.gameObject.SetActive(true);
                }));
                _dicDialog.Add(path,newDialog);
                return newDialog;
            }
            else
            {
                Debug.LogError($"Cant find PATH:{path}");
            }
        }
        else
        {
            BaseDialog dialog = _dicDialog[path];
            StartCoroutine(DelayAction(delayActive, () =>
            {
                dialog.gameObject.SetActive(true);
            }));
            return dialog.GetComponent<T>();
        }

        return null;
    }

    IEnumerator DelayAction(float time,Action t)
    {
        yield return new WaitForSeconds(time);
        t?.Invoke();
    }

    public async void DisableAllDialog()
    {
        _transition.Play("Transition"); ;
        await UniTask.Delay(TimeSpan.FromSeconds(0.5F),cancellationToken: this.GetCancellationTokenOnDestroy());
        foreach (var dialog in _dicDialog)
        {
            dialog.Value.gameObject.SetActive(false);
        }
    }


    //PRIVATE
    private Dictionary<string, BaseDialog> _dicDialog;
    [Tooltip("Can custome this if using in another project")] 
    [Header("FX")]
    [SerializeField] private Animator _transition;
    
}


[Serializable]
public class DialogContainer
{
    public List<Dialog> container;

    public Dialog this[DialogType type]
    {
        get
        {
            var dialog = container.Find(x => x.dialogType == type);
            return dialog;
        }
    }

}


[Serializable]
public class Dialog
{
    public Transform trans;
    public DialogType dialogType;
}

public enum DialogType
{
    DialogWithNavigate,
    DialogWithoutNavigate
}