const username = 'zangassis'
const perPage = 45; //If you have more articles, increase this or add pagination

const getArticles = async() => {
    const response = await fetch(`https://<your-app-name>.herokuapp.com/v1/posts/recent`);
    const data = await response.json();
    const name = data[0].author.name;
    document.querySelectorAll('.name').forEach(el => el.textContent = name);
    document.title = `Blog - ${name}`;
    for (article of data) {
        addArticle(article);
    }
}

const addArticle = article => {
    const template = document.querySelector('#blog-item');
    const clone = template.content.cloneNode(true);
    clone.querySelector('.title').textContent = article.title;
    clone.querySelector('.url').href = `article.html?id=${article.id}`;

    if (article.coverImage) {
        clone.querySelector('.cover').src = article.coverImage;
        console.log(article.coverImage);
    } else {
        clone.querySelector('.cover').src = './assets/placeholder.jpg';
    }

    document.querySelector('#blog-list').appendChild(clone);
}

getArticles();