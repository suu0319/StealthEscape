using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Mediator;
using DG.Tweening;

namespace Puzzle 
{
	public class PintuPuzzleController : MonoBehaviour
	{
		[SerializeField]
		private GameObject _joyStick, _attackBtnObj, _optionBtnObj, _eventBtnObj, _closeBtnObj, _pintuPuzzleObj;

		[SerializeField]
		private GameObject[] _correctPintu;

		[SerializeField]
		private Button _closeBtn;

		private bool isChange = false;

		private void Awake()
		{
			AddOnClickListener();
		}

        private void OnEnable()
        {
			Init();
		}

        private void Update()
		{
			OperatePintu();
		}

		/// <summary>
		/// 開啟初始化
		/// </summary>
        private void Init()
        {
			_joyStick.SetActive(false);
			_attackBtnObj.SetActive(false);
			_optionBtnObj.SetActive(false);
			_eventBtnObj.SetActive(false);
			_closeBtnObj.SetActive(true);

			Time.timeScale = 0f;
		}

		/// <summary>
		/// 註冊Close Button OnClick事件
		/// </summary>
		private void AddOnClickListener()
		{
			_closeBtn.onClick.AddListener(QuitPintuPuzzle);
		}

		/// <summary>
		/// 退出拼圖謎題
		/// </summary>
		private void QuitPintuPuzzle()
		{
			Time.timeScale = 1f;

			_joyStick.SetActive(true);
			_attackBtnObj.SetActive(true);
			_optionBtnObj.SetActive(true);
			_eventBtnObj.SetActive(true);
			_closeBtnObj.SetActive(false);
			gameObject.SetActive(false);
		}

        /// <summary>
        /// 操作拼圖
        /// </summary>
        private void OperatePintu()
		{
			if (Input.touchCount > 0 && !isChange)
			{
				Touch touch = Input.GetTouch(0);

				PointerEventData pointer = new PointerEventData(EventSystem.current);
				pointer.position = Input.mousePosition;

				List<RaycastResult> raycastResults = new List<RaycastResult>();
				EventSystem.current.RaycastAll(pointer, raycastResults);

				if (touch.phase == TouchPhase.Moved && raycastResults.Count > 0)
				{
					if (raycastResults[0].gameObject.tag == "PintuUI") 
					{
						raycastResults[0].gameObject.transform.position = Input.mousePosition;
					}

					if (raycastResults.Count == 2)
					{
						int temp = raycastResults[0].gameObject.transform.GetSiblingIndex();
						raycastResults[0].gameObject.transform.SetSiblingIndex(raycastResults[1].gameObject.transform.GetSiblingIndex());
						raycastResults[1].gameObject.transform.SetSiblingIndex(temp);

						if (VerifyPintuPuzzle()) 
						{
							Debug.Log("拼圖謎題過關");
							QuitPintuPuzzle();

							_eventBtnObj.SetActive(false);
							_pintuPuzzleObj.SetActive(false);

							DesertGameSceneMediator.Instance.PintuPuzzleTrigger.AudioSource.Play();
						}
                        else 
						{
							isChange = true;

							var time = 0;
							DOTween.To(() => time, x => time = x, 1, 1f).SetUpdate(true).onComplete += (() => isChange = false);
						}
					}				
				}

				if (touch.phase == TouchPhase.Ended)
				{
					LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
				}
			}
		}

		/// <summary>
		/// 驗證拼圖謎題是否通關
		/// </summary>
		/// <returns></returns>
		private bool VerifyPintuPuzzle()
		{
			for (int i = 0; i < _correctPintu.Length; i++) 
			{
				if (i == _correctPintu.Length - 1) 
				{
					return true;
				}
				else if (gameObject.transform.GetChild(i).gameObject == _correctPintu[i])
				{
					continue;
				}
                else 
				{
					break;
				}
			}

			return false;
		}
	}
}