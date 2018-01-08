using UnityEngine;
using UnityEngine.SceneManagement;

namespace BattleInterface
{
	[RequireComponent(typeof(ShipMovement))]
	[RequireComponent(typeof(ShipBehaviour))]
	public class KeyboardController : MonoBehaviour
	{
		private ShipMovement shipMovement;
		private ShipBehaviour shipBehaviour;

		void Start()
		{
			shipMovement = GetComponent<ShipMovement>();
			shipBehaviour = GetComponent<ShipBehaviour>();
		}

		void Update()
		{
			bool isUpPressed = (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W));
			bool isDownPressed = (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S));

			if (isUpPressed && !isDownPressed)
			{
				shipMovement.AccelerateUp();
			}
			if (isDownPressed && !isUpPressed)
			{
				shipMovement.AccelerateDown();
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (shipMovement.IsStopped())
				{
					SceneManager.LoadScene("MainMenu");
				}
				else
				{
					shipBehaviour.Shoot();
				}
			}
		}
	}
}
