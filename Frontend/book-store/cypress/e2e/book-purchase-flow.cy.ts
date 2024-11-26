describe('Book Purchase Flow', () => {
  beforeEach(() => {
    cy.visit('/catalog');
  });

  it('should allow a user to log in, add a book to cart, and modify cart quantity', () => {
    // Step 1: Log in
    cy.get('.user-icon').click();
    cy.get('#username').type('user');
    cy.get('#password').type('1111');
    cy.get('button[type="submit"]').click();

    // Step 2: Navigate to catalog
    cy.get('a[routerLink="/catalog"]').click();

    // Step 3: Add a book to cart
    cy.get('.book-card').first().within(() => {
      cy.get('button').contains('Add to Cart').click();
    });

    cy.get('.p-toast-message-content').should('contain', 'Book added to cart');

    // Step 4: Go to cart
    cy.get('.cart-icon').click();

    cy.get('.cart-header h2').should('contain', 'Cart');

    // Step 5: Change the count of books
    cy.get('.quantity-control').first().within(() => {
      cy.get('span').invoke('text').then((initialQuantity) => {
        cy.get('button').last().click();

        cy.get('span').should('have.text', (parseInt(initialQuantity) + 1).toString());
      });
    });

    cy.get('.last-total span').last().invoke('text').then((totalText) => {
      const total = parseFloat(totalText.replace('$', ''));
      expect(total).to.be.greaterThan(0);
    });
  });
});