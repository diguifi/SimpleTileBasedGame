using System.Collections;
using UnityEngine;

namespace Diguifi.Elements
{
    public class PlayerTiled : MonoBehaviour
    {

        Direction currentDir;
        Vector2 input;
        Vector3 startPos;
        Vector3 endPos;
        bool isMoving = false;
        float time;

        bool colliding = false;
        bool collidingRight = false;
        bool collidingLeft = false;
        bool collidingUp = false;
        bool collidingDown = false;

        public Sprite spriteUp;
        public Sprite spriteRight;
        public Sprite spriteDown;
        public Sprite spriteLeft;

        public float walkSpeed = 3f;

        public bool isAllowedToMove = true;

        public Grid grid;

        void Start()
        {
            isAllowedToMove = true;
        }

        void Update()
        {

            if (!isMoving && isAllowedToMove)
            {
                GetInput();

                if (input != Vector2.zero)
                {
                    PlayerMovement();
                }

            }

        }

        public void GetInput()
        {
            input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
                input.y = 0;
            else
                input.x = 0;
        }

        public void PlayerMovement()
        {
            if (input.x < 0)
            {
                currentDir = Direction.Left;
            }
            if (input.x > 0)
            {
                currentDir = Direction.Right;
            }
            if (input.y < 0)
            {
                currentDir = Direction.Down;
            }
            if (input.y > 0)
            {
                currentDir = Direction.Up;
            }

            ChangeSprite();

            if (!colliding)
                StartCoroutine(Move(transform));
            else
            {
                if (collidingRight && currentDir == Direction.Right)
                    input.x = 0;
                if (collidingLeft && currentDir == Direction.Left)
                    input.x = 0;
                if (collidingUp && currentDir == Direction.Up)
                    input.y = 0;
                if (collidingDown && currentDir == Direction.Down)
                    input.y = 0;
                StartCoroutine(Move(transform));
            }
        }

        public void ChangeSprite()
        {
            switch (currentDir)
            {
                case Direction.Up:
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteUp;
                    break;
                case Direction.Right:
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteRight;
                    break;
                case Direction.Down:
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteDown;
                    break;
                case Direction.Left:
                    gameObject.GetComponent<SpriteRenderer>().sprite = spriteLeft;
                    break;
            }
        }

        public IEnumerator Move(Transform entity)
        {
            isMoving = true;
            startPos = entity.position;
            time = 0;

            endPos = new Vector3(startPos.x + System.Math.Sign(input.x), startPos.y + System.Math.Sign(input.y), startPos.z);

            while (time < 1f)
            {
                time += Time.deltaTime * walkSpeed;
                entity.position = Vector3.Lerp(startPos, endPos, time);
                yield return null;
            }

            isMoving = false;
            Vector3 worldPoint = entity.position;
            Debug.Log(grid.WorldToCell(worldPoint));
            yield return 0;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            colliding = true;
            if (currentDir == Direction.Right)
                collidingRight = true;
            else if (currentDir == Direction.Left)
                collidingLeft = true;
            else if (currentDir == Direction.Up)
                collidingUp = true;
            else if (currentDir == Direction.Down)
                collidingDown = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            colliding = false;
        }
    }

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }
}