const buttons = document.querySelectorAll(".heart-like-button");

for(let i = 0; i < buttons.length; i++)
{
  buttons[i].addEventListener("click", () => {
    if (buttons[i].classList.contains("liked")) {
      buttons[i].classList.remove("liked");
    } else {
      buttons[i].classList.add("liked");
    }
  });
}
