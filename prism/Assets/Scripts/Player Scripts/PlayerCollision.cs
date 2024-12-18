using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public AudioClip playerDamageAudio;
    public bool OnCooldown = false;

    void OnCollisionEnter2D(Collision2D collision)
    {

        // checks for collision with bullet reduces health destroys bullet
        if (collision.gameObject.tag.Equals("EnemyBullet"))
        {   
            print("COLLISION");
            Destroy(collision.gameObject);
            // reduce hp by 1 when hit by bullet

            if (OnCooldown) {return;};
            gameObject.GetComponent<PlayerController>().hp -= 1;
            StartCoroutine(CooldownCycle());
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
        }

        // checks for collision with enemy reduces health

        if (collision.gameObject.tag.Equals("Enemy"))
        {
            // collision.gameObject.takeDamage()
            if (OnCooldown) {return;};
            gameObject.GetComponent<PlayerController>().hp -= 1;
            StartCoroutine(CooldownCycle());
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
            // If ^ doesn't work, just GameObject.findAnyObjectOfType<>
        }


        //checks for collision with ouch or ouchobstacle reduces health
        if (collision.gameObject.tag.Equals("OuchObstacle"))
        {
            // collision.gameObject.takeDamage()
            if (OnCooldown) {return;};
            if (gameObject.GetComponent<PlayerController>().ouchImmunity) {return;};
            gameObject.GetComponent<PlayerController>().hp -= 1;
            StartCoroutine(CooldownCycle());
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
            // If ^ doesn't work, just GameObject.findAnyObjectOfType<>
        }

    }

    

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Ouch"))
        {
            if (OnCooldown) {return;};
            if (gameObject.GetComponent<PlayerController>().ouchImmunity) {return;};
            gameObject.GetComponent<PlayerController>().hp -= 1;
            StartCoroutine(CooldownCycle());
            gameObject.GetComponent<PlayerController>().audioSource.PlayOneShot(playerDamageAudio);
        }

        if (collision.gameObject.tag.Equals("Shard"))
        {
            gameObject.GetComponent<PlayerController>().numShards += 1;
            GameObject.Find("ShardCountText").GetComponent<UnityEngine.UI.Text>().text = gameObject.GetComponent<PlayerController>().numShards + " / " + gameObject.GetComponent<PlayerController>().totalShards;
            Destroy(collision.gameObject);
            if (gameObject.GetComponent<PlayerController>().numShards == gameObject.GetComponent<PlayerController>().totalShards)
            {
                GameObject.Find("WinCrystal").GetComponent<SpriteRenderer>().enabled = true;
                GameObject.Find("WinCrystal").GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        
        if (collision.gameObject.name == "FullHealItem")
        {
            if (gameObject.GetComponent<PlayerController>().hp == gameObject.GetComponent<PlayerController>().maxHealth) {return;};
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().hp = gameObject.GetComponent<PlayerController>().maxHealth;
        }

        if (collision.gameObject.name == "PartialHealItem")
        {
            if (gameObject.GetComponent<PlayerController>().hp == gameObject.GetComponent<PlayerController>().maxHealth) {return;};
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().hp += 1;
        }

        if (collision.gameObject.name == "PowerBuffItem")
        {
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().damageMultiplier *= 1.5f;
            StartCoroutine(PowerBuffTimer());
        }

        if (collision.gameObject.name == "SpeedBuffItem")
        {
            Destroy(collision.gameObject);
            gameObject.GetComponent<PlayerController>().moveSpeed *= 1.4f;
            StartCoroutine(SpeedBuffTimer());
        }
        
    }

    IEnumerator SpeedBuffTimer()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ItemPanel"))
        {
            if (obj == gameObject || obj.name == "ItemSpeedPanel" || obj.name == "ItemPowerPanel") {continue;};
            RectTransform rectT = obj.GetComponent<RectTransform>();
            rectT.anchoredPosition = new Vector2(rectT.anchoredPosition.x, rectT.anchoredPosition.y + 40);
        }
        
        GameObject panel = Instantiate(GameObject.Find("ItemSpeedPanel"), GameObject.Find("Canvas").transform);
        panel.transform.GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("IconBG").GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("IconBG").transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("DurationBar").GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("EndPos").GetComponent<UnityEngine.UI.Image>().enabled = true;

        panel.transform.Find("DurationBar").GetComponent<RectTransform>().DOAnchorPos(panel.transform.Find("EndPos").GetComponent<RectTransform>().anchoredPosition, 20f).SetEase(Ease.Linear);
        panel.transform.Find("DurationBar").GetComponent<RectTransform>().DOSizeDelta(panel.transform.Find("EndPos").GetComponent<RectTransform>().sizeDelta, 20f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(20f);
        gameObject.GetComponent<PlayerController>().moveSpeed /= 1.4f;
        Destroy(panel);
    }

    IEnumerator PowerBuffTimer()
    {
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("ItemPanel"))
        {
            if (obj == gameObject || obj.name == "ItemSpeedPanel" || obj.name == "ItemPowerPanel") {continue;};
            RectTransform rectT = obj.GetComponent<RectTransform>();
            rectT.anchoredPosition = new Vector2(rectT.anchoredPosition.x, rectT.anchoredPosition.y + 40);
        }
        
        GameObject panel = Instantiate(GameObject.Find("ItemPowerPanel"), GameObject.Find("Canvas").transform);
        panel.transform.GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("IconBG").GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("IconBG").transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("DurationBar").GetComponent<UnityEngine.UI.Image>().enabled = true;
        panel.transform.Find("EndPos").GetComponent<UnityEngine.UI.Image>().enabled = true;

        panel.transform.Find("DurationBar").GetComponent<RectTransform>().DOAnchorPos(panel.transform.Find("EndPos").GetComponent<RectTransform>().anchoredPosition, 20f).SetEase(Ease.Linear);
        panel.transform.Find("DurationBar").GetComponent<RectTransform>().DOSizeDelta(panel.transform.Find("EndPos").GetComponent<RectTransform>().sizeDelta, 20f).SetEase(Ease.Linear);

        yield return new WaitForSeconds(20f);
        gameObject.GetComponent<PlayerController>().damageMultiplier /= 1.5f;
        Destroy(panel);
    }

    IEnumerator CooldownCycle()
    {
        OnCooldown = true;
        for (int i = 0; i < 20; i++)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            yield return new WaitForSeconds(0.05f);
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            yield return new WaitForSeconds(0.05f);
        }
        OnCooldown = false;
    }
}


