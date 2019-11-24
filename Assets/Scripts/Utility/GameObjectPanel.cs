using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameObjectPanel : MonoBehaviour
{
    public Text firstText;
    public List<Text> restOfText = new List<Text>();
    public List<Transform> transforms = new List<Transform>();
    public Coroutine updatePanel;

    private void OnEnable()
    {
        updatePanel = StartCoroutine(UpdatePanel());
    }

    private void OnDisable()
    {
        if (updatePanel != null) StopCoroutine(updatePanel);
        updatePanel = null;
    }

    private IEnumerator UpdatePanel()
    {
        while (true)
        {
            transforms.Clear();
            transforms.AddRange(FindObjectsOfType<Transform>().Where(t => NotMyChild(t)));

            for (int i = restOfText.Count; i < transforms.Count - 1; i++)
            {
                var newGameObject = Instantiate(firstText);
                var newText = newGameObject.GetComponent<Text>();
                newGameObject.transform.SetParent(firstText.transform.parent);
                restOfText.Add(newText);
            }

            for (int i = restOfText.Count - 1; i >= transforms.Count; i--)
            {
                Destroy(restOfText[i].gameObject);
                restOfText.RemoveAt(i);
            }

            for (int i = 0; i < transforms.Count - 1; i++)
                if (i == 0) firstText.text = GetText(transforms[i]);
                else restOfText[i - 1].text = GetText(transforms[i]);
            yield return new WaitForSeconds(0.25f);
        }
    }

    private string GetText(Transform transform)
    {
        var s = transform.GetComponent<SpriteRenderer>();
        if (!s) return $"{transform.name}";
        else
        {
            return $"{transform.name}\nSprite: {s.sprite}";
            //var renderer = transform.GetComponent<SpriteRenderer>();
            //if (!renderer)
            //    return $"{transform.name}\n---\n---\n---\n---";
            //return $"{transform.name}\n"
            //    + $"Sprite: {renderer.sprite.name}\n"
            //    + $"Material: {renderer.material.name}\n"
            //    //+ $"X: {transform.position.x:0.00} "
            //    //+ $"Y: {transform.position.y:0.00} "
            //    //+ $"Z: {transform.position.z:0.00}";
            //    //+ $"X: {transform.eulerAngles.x:0.00} "
            //    //+ $"Y: {transform.eulerAngles.y:0.00} "
            //    //+ $"Z: {transform.eulerAngles.z:0.00}\n"
            //+ $"X: {transform.localScale.x:0.00} "
            //+ $"Y: {transform.localScale.y:0.00} "
            //+ $"Z: {transform.localScale.z:0.00}";
        }
    }

    private bool NotMyChild(Transform transform)
    {
        var parent = transform.parent;
        if (transform == this.transform) return false;
        if (parent == this.transform) return false;
        if (parent == null) return true;
        return NotMyChild(parent);
    }
}
